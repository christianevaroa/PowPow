using UnityEngine;
using System.Collections;

namespace PlayerScripts
{

    public class PlayerStatus : MonoBehaviour
    {
        PlayerMovementRB movement;
        [Tooltip("Set the player's number here (1-4)")]
        public int playerNumber;

        // Use this for initialization
        void Start()
        {
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
            movement.EnableMovement();
        }
    }

}
