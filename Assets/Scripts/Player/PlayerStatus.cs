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
        PlayerAudio audioScript;

        public bool debugging;

        [Tooltip("Set the player's number here (1-4)")]
        public int playerNumber;
        public int startingHealth;
        public int health;
        public ControlState controlState { get; private set; }

        // Use this for initialization
        void Start()
        {
            PlayerStatus.playerCount++;
            controlState = ControlState.NOT_CONTROLLABLE;
            movement = GetComponent<PlayerMovementRB>();
            audioScript = GetComponent<PlayerAudio>();

            if (playerNumber <= 0 || playerNumber > 4)
            {
                Debug.LogError(this + ": playerNumber should be 1 to 4, was: " + playerNumber);
            }

            health = startingHealth;
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

        public void TakeDamage(Damage d)
        {
            Debug.Log(gameObject.name + " took " + d.amount + " " + d.type + " damage.");
            if ((health -= d.amount) <= 0)
            {
                health = 0;
                Die();
            }
            else
            {
                audioScript.GetHit();
            }
        }

        public void Die()
        {
            //TODO: Die!
            Debug.Log(this + " died.");
            controlState = ControlState.NOT_CONTROLLABLE;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;
            audioScript.Die();
            RoundManager.Death();
        }

        public void Dance()
        {
            movement.anim.SetTrigger("Dance");
        }
    }
}
