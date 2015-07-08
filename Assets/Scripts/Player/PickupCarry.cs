using UnityEngine;
using System.Collections;

namespace PlayerScripts 
{
    public class PickupCarry : MonoBehaviour
    {

        [HideInInspector]
        public string playerName, interactButton;

        public float raycastDistance;
        public Transform carryPosition;

        ThrowableMono heldObject;
        Transform heldObjectTransform;
        PlayerStatus status;

        public ControlState controlState { get { return status.controlState; } }
        public CarryState carryState { get { return status.carryState; } }

        // Use this for initialization
        void Start()
        {
            status = GetComponent<PlayerStatus>();
            if (raycastDistance == 0f)
            {
                Debug.LogWarning(this + ": Set raycastDistance in inspector");
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown(interactButton) && controlState == ControlState.CONTROLLABLE)
            {
                Interact();
            }
        }

        public void Interact()
        {
            if (carryState == CarryState.NOT_CARRYING)
            {
                if (status.debugging)
                {
                    Debug.DrawRay(transform.position + transform.up * 0.5f, transform.forward * raycastDistance, Color.magenta, 0.3f);
                }

                RaycastHit hit;
                Ray pickupRay = new Ray(transform.position + transform.up * 0.5f, transform.forward);
                if (Physics.Raycast(pickupRay, out hit, raycastDistance))
                {
                    if (hit.collider.tag == "Throwable")
                    {
                        GameObject obj = hit.collider.gameObject;
                        Debug.Log(hit.collider.gameObject + ", " + obj);
                        heldObject = obj.GetComponent<ThrowableMono>();
                        heldObjectTransform = obj.transform;
                        status.carryState = CarryState.CARRYING;
                        heldObject.BeInteractedWith(gameObject, carryPosition);

                        Debug.Log("Picked up: " + obj + ",\n"
                            + obj.tag + "\n" + status.carryState);
                    }
                }

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
            interactButton = "Interact_" + playerName;
        }
    }

}
