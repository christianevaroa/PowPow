using UnityEngine;
using System.Collections;

namespace PlayerScripts
{
    public enum ControlState { CONTROLLABLE, NOT_CONTROLLABLE }
    public enum CarryState { NOT_CARRYING, CARRYING }
    public class PlayerStatus : MonoBehaviour
    {
        public static int playerCount;

        PlayerMovementRB movement;

        public bool debugging;

        [Tooltip("Set the player's number here (1-4)")]
        public int playerNumber;

        
        public ControlState controlState { get; private set; }

        // Use this for initialization
        void Start()
        {
            PlayerStatus.playerCount++;
            controlState = ControlState.NOT_CONTROLLABLE;
            movement = GetComponent<PlayerMovementRB>();

            if (playerNumber <= 0 || playerNumber > 4)
            {
                Debug.LogError(this + ": playerNumber should be 1 to 4, was: " + playerNumber);
            }

            movement.SetPlayer(playerNumber);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartRound()
        {
            EnableMovement();
            // Do some other stuff
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
