using UnityEngine;
using System.Collections;

namespace PlayerScripts 
{
    public class PickupCarry : MonoBehaviour
    {

        [HideInInspector]
        public string playerName, interactButton;

        ThrowableMono heldObject;
        Transform heldObjectTransform;
        PlayerStatus status;

        public PlayerStatus.ControlState controlState { get { return status.controlState; } }
        public PlayerStatus.CarryState carryState { get { return status.carryState; } }

        // Use this for initialization
        void Start()
        {
            status = GetComponent<PlayerStatus>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown(interactButton) && controlState == PlayerStatus.ControlState.CONTROLLABLE)
            {

            }
        }

        void Interact()
        {

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
