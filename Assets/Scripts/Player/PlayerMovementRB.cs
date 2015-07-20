using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace PlayerScripts
{
    /// <summary>
    /// Rigidbody-based player movement.
    /// Uses a state machine & a state pool.
    /// </summary>
    public class PlayerMovementRB : MonoBehaviour
    {
        public Text debugText;
        public bool debugging { get { return status.debugging; } }

        PlayerAudio audioScript;
        PlayerStatus status;
        public ControlState controlState { get { return status.controlState; } }

        public Animator anim;

        // Movement stuff
        public float maxSpeed;
        [Tooltip("Don't set this, it's set to half max speed in Start()")]
        public float maxCrawlSpeed;
        public float rotateSpeed;
        public float groundRaycastDistance, groundRaycastHeight;
        [HideInInspector]
        public Vector3 directionVector, lastNonZeroVector;
        [HideInInspector]
        public Rigidbody rb;
        [HideInInspector]
        public CapsuleCollider col;

        // Movement state machine
        public PlayerStatePool statePool { get; private set; }
        IPlayerState movementState;
        

        public float currentSpeed { get; private set; }
        public float xMovement { get; private set; }
        public float zMovement { get; private set; }
        public float maxVelocityChange { get; private set; }
        [HideInInspector]
        public bool crouching;
        public bool grounded { get; private set; }

        // Picking up & carrying stuff
        public float pickupRaycastDistance;
        public float throwStrength;
        public Transform carryPosition;
        public Throwable heldObject;
        private CarryState hiddenCarryState;
        public CarryState carryState { get { return hiddenCarryState; } private set { hiddenCarryState = value; } }

        // Strings for storing Input strings
        [HideInInspector]
        public string playerName, jumpButton, interactButton, horizontalAxis, verticalAxis;

        // Use this for initialization
        void Start()
        {
            status = GetComponent<PlayerStatus>();
            statePool = new PlayerStatePool();
            rb = GetComponent<Rigidbody>();
            col = GetComponent<CapsuleCollider>();
            audioScript = GetComponent<PlayerAudio>();
            grounded = true;
            movementState = statePool.GetState("IDLE");
            carryState = CarryState.NOT_CARRYING;

            directionVector = Vector3.zero;

            maxCrawlSpeed = maxSpeed / 2f;
            maxVelocityChange = 3f;
        }

        // Update is called once per frame
        void Update()
        {
            CheckGrounded();

            xMovement = Input.GetAxis(horizontalAxis);
            zMovement = Input.GetAxis(verticalAxis);
            directionVector.Set(xMovement, 0f, zMovement);
            directionVector = Vector3.ClampMagnitude(directionVector * maxSpeed, maxSpeed);
            currentSpeed = directionVector.magnitude;

            if (currentSpeed > 0.0000001) 
            {
                lastNonZeroVector = directionVector;
            }

            movementState = SwitchState(movementState.Update(this));

            if (debugging)
                PrintDebug();
        }

        void FixedUpdate()
        {
            movementState = SwitchState(movementState.ProcessMovement(this));
            movementState = SwitchState(movementState.ProcessJump(this));
        }

        void OnAnimatorIK()
        {
            // Set the hand positions when carrying an object
            if (carryState == CarryState.CARRYING)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                anim.SetIKPosition(AvatarIKGoal.RightHand, heldObject.rightIKHandle.position);

                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                anim.SetIKPosition(AvatarIKGoal.LeftHand, heldObject.leftIKHandle.position);
            }
            else
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            }
        }

        IPlayerState SwitchState(IPlayerState newState)
        {
            if (newState != movementState)
            {
                newState.EnterState(this);
            }
            return newState;
        }

        private void CheckGrounded()
        {
            grounded = Raycast(transform.position) || Raycast(transform.position + transform.forward * col.radius)
                || Raycast(transform.position + transform.forward * -col.radius) || Raycast(transform.position + transform.right * col.radius)
                || Raycast(transform.position + transform.right * -col.radius);
        }

        bool Raycast(Vector3 pos)
        {
            Ray ray = new Ray(pos + (transform.up * groundRaycastHeight), (-transform.up * groundRaycastDistance));

            if (debugging)
            {
                // Why no DrawRay(Ray) method? Unity pls
                Debug.DrawRay(pos + (transform.up * groundRaycastHeight), (-transform.up * groundRaycastDistance));
            }

            RaycastHit hit;
            return Physics.Raycast(ray, out hit, groundRaycastDistance);
        }

        public void RotateToFace()
        {
            // Rotate the player towards the direction they're moving
            if (lastNonZeroVector != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lastNonZeroVector);
                targetRotation = Quaternion.RotateTowards(rb.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
                rb.MoveRotation(targetRotation);
            }
        }

        public void Interact()
        {
            if (carryState == CarryState.NOT_CARRYING)
            {
                if (status.debugging)
                {
                    Debug.DrawRay(transform.position + transform.up * 0.5f, transform.forward * pickupRaycastDistance, Color.magenta, 0.3f);
                }

                RaycastHit hit;
                Ray pickupRay = new Ray(transform.position + transform.up * 0.5f, transform.forward);
                if (Physics.Raycast(pickupRay, out hit, pickupRaycastDistance))
                {
                    Debug.Log(hit.collider);
                    if (hit.collider.tag == "Throwable")
                    {
                        // Object is a throwable, pick it up
                        GameObject obj = hit.collider.gameObject;
                        heldObject = obj.GetComponent<Throwable>();
                        carryState = CarryState.CARRYING;
                        heldObject.BeInteractedWith(gameObject, carryPosition);
                    }
                }
            }
            else if (carryState == CarryState.CARRYING)
            {
                carryState = CarryState.NOT_CARRYING;
                heldObject.GetThrown(transform.forward * throwStrength);
                heldObject = null;
                audioScript.Throw();
            }
        }

        /// <summary>
        /// Called by PlayerStatus.Start() to assign the player's number and set up controls
        /// </summary>
        /// <param name="pnum">Should be between 1 and 4</param>
        public void SetPlayer(int playerNumber)
        {
            if (playerNumber <= 0 || playerNumber > 4)
            {
                Debug.LogError(this + ": playerNumber should be 1 to 4, was: " + playerNumber);
            }
            playerName = "P" + playerNumber;
            jumpButton = "Jump_" + playerName;
            interactButton = "Interact_" + playerName;
            horizontalAxis = "Horizontal_" + playerName;
            verticalAxis = "Vertical_" + playerName;

        }

        void PrintDebug()
        {
            if (debugText != null)
            {
                debugText.text =
                                "movementState = " + movementState.GetType() + "\n" +
                                //"xMovement = " + xMovement + "\n" +
                                //"zMovement = " + zMovement + "\n" +
                                //"directionVector = " + directionVector + "\n" +
                                //"airborneDirectionVector = " + airborneDirectionVector + "\n" +
                                //"currentSpeed = " + currentSpeed + "\n" +
                                "rb.velocity.magnitude = " + rb.velocity.magnitude + "\n" +
                                //"grounded = " + grounded + "\n" +
                                //"friction = " + col.material.dynamicFriction +
                                "";
            }
            else
            {
                Debug.LogWarning("Tried to update debugText but PlayerMovementRB.debugText is not set");
            }
            
        }
    }

}