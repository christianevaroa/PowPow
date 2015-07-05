using UnityEngine;
using System.Collections;

namespace PlayerScripts
{

    public class PlayerStatus : MonoBehaviour
    {
        PlayerMovementRB movement;
        PickupCarry carry;

        [Tooltip("Set the player's number here (1-4)")]
        public int playerNumber;

        public enum ControlState { CONTROLLABLE, NOT_CONTROLLABLE }
        public ControlState controlState { get; private set; }

        public enum CarryState { NOT_CARRYING, CARRYING }
        public CarryState carryState { get; private set; }

        // Use this for initialization
        void Start()
        {
            controlState = ControlState.NOT_CONTROLLABLE;
            movement = GetComponent<PlayerMovementRB>();
            carry = GetComponent<PickupCarry>();

            if (playerNumber <= 0 || playerNumber > 4)
            {
                Debug.LogError(this + ": playerNumber should be 1 to 4, was: " + playerNumber);
            }

            movement.SetPlayer(playerNumber);
            carry.SetPlayer(playerNumber);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartRound()
        {
            controlState = ControlState.CONTROLLABLE;
            movement.EnableMovement();
        }

        public void EnableMovement()
        {
            controlState = ControlState.CONTROLLABLE;
        }

        public void DisableMovement()
        {
            controlState = ControlState.NOT_CONTROLLABLE;
        }
    }

}
