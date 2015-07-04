using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace PlayerScripts
{

    public class PlayerMovementRB : MonoBehaviour
    {

        public Text debugText;
        public bool debugging;

        public Animator anim;

        public float maxSpeed;
        public float maxCrawlSpeed;
        public float gravity;
        public float rotateSpeed;
        public float groundRaycastDistance;
        public float groundRaycastHeight;

        public Vector3 directionVector;
        public Vector3 airborneDirectionVector;

        public Rigidbody rb;
        public CapsuleCollider col;

        public float currentSpeed { get; private set; }
        public float xMovement { get; private set; }
        public float zMovement { get; private set; }
        public float maxVelocityChange { get; private set; }
        public bool crouching;
        public bool grounded { get; private set; }

        public enum CarryState { NOT_CARRYING, CARRYING }
        public enum ControlState { CONTROLLABLE, NOT_CONTROLLABLE }

        public CarryState carryState { get; private set; }
        public ControlState controlState { get; private set; }
        private IPlayerState movementState;
        public PlayerStatePool statePool { get; private set; }

        // Use this for initialization
        void Start()
        {
            statePool = new PlayerStatePool();
            rb = GetComponent<Rigidbody>();
            col = GetComponent<CapsuleCollider>();
            grounded = true;
            carryState = CarryState.NOT_CARRYING;
            controlState = ControlState.NOT_CONTROLLABLE;
            movementState = statePool.GetState("IDLE");

            directionVector = Vector3.zero;
            airborneDirectionVector = Vector3.zero;

            maxCrawlSpeed = maxSpeed / 2f;
            maxVelocityChange = 3f;
        }

        // Update is called once per frame
        void Update()
        {
            CheckGrounded();

            xMovement = Input.GetAxis("Horizontal");
            zMovement = Input.GetAxis("Vertical");
            directionVector.Set(xMovement, 0f, zMovement);
            directionVector = Vector3.ClampMagnitude(directionVector * maxSpeed, maxSpeed);
            currentSpeed = directionVector.magnitude;

            movementState = SwitchState(movementState.Update(this));

            if (debugging)
                PrintDebug();
        }

        void FixedUpdate()
        {
            movementState = SwitchState(movementState.ProcessMovement(this));
            movementState = SwitchState(movementState.ProcessJump(this));
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

        public void EnableMovement()
        {
            controlState = ControlState.CONTROLLABLE;
        }

        public void DisableMovement()
        {
            controlState = ControlState.NOT_CONTROLLABLE;
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
                                //"rb.velocity.magnitude = " + rb.velocity.magnitude + "\n" +
                                "grounded = " + grounded + "\n" +
                                "friction = " + col.material.dynamicFriction +
                                "";
            }
            else
            {
                Debug.LogWarning("Tried to update debugText but PlayerMovementRB.debugText is not set");
            }
            
        }
    }

}