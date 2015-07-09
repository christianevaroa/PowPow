using UnityEngine;
using System.Collections;

namespace PlayerScripts 
{
    public class PickupCarry : MonoBehaviour
    {

        [HideInInspector]
        public string playerName, interactButton;

        public float raycastDistance;
        public float throwStrength;
        public Transform carryPosition;

        ThrowableMono heldObject;
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
                    Debug.Log(hit.collider);
                    if (hit.collider.tag == "Throwable")
                    {
                        // Object is a throwable, pick it up
                        GameObject obj = hit.collider.gameObject;
                        heldObject = obj.GetComponent<ThrowableMono>();
                        status.carryState = CarryState.CARRYING;
                        heldObject.BeInteractedWith(gameObject, carryPosition);
                    }
                }
            }
            else if (carryState == CarryState.CARRYING)
            {
                status.carryState = CarryState.NOT_CARRYING;
                Debug.Log(transform.forward);
                heldObject.GetThrown(transform.forward * throwStrength);
                heldObject = null;
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
