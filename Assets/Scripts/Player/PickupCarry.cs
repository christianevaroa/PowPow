using UnityEngine;
using System.Collections;

namespace PlayerScripts 
{
    public class PickupCarry : MonoBehaviour
    {

        [HideInInspector]
        public string playerName, interactButton;

        public float raycastDistance;

        ThrowableMono heldObject;
        Transform heldObjectTransform;
        PlayerStatus status;

        public PlayerStatus.ControlState controlState { get { return status.controlState; } }
        public PlayerStatus.CarryState carryState { get { return status.carryState; } }

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
            if (Input.GetButtonDown(interactButton) && controlState == PlayerStatus.ControlState.CONTROLLABLE)
            {
                Debug.Log("got here " + interactButton);
                Interact();
            }
        }

        void Interact()
        {
            if (carryState == PlayerStatus.CarryState.NOT_CARRYING)
            {
                Debug.DrawRay(gameObject.transform.position + gameObject.transform.up * 0.5f, gameObject.transform.forward * raycastDistance, Color.magenta, 0.3f);
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
