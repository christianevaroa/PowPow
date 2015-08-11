using UnityEngine;

namespace PlayerScripts
{

    public class PlayerStateMoving : IPlayerState
    {

        string hiddenName;

        public string name { get { return hiddenName; } set { } }

        public PlayerStateMoving()
        {
            hiddenName = "MOVING";
        }

        void IPlayerState.EnterState(PlayerMovementRB player)
        {
            player.anim.SetFloat("Speed", player.currentSpeed);
            player.SetCrouching(false);
        }

        IPlayerState IPlayerState.Update(PlayerMovementRB player)
        {
            // Check if the player jumped BEFORE we check if they aren't grounded
            // so if both happen in the same frame they can still jump
            if (Input.GetButtonDown(player.jumpButton))
            {
                return player.statePool.GetState("JUMPING");
            }
            else if (!player.grounded)
            {
                return player.statePool.GetState("FALLING");
            }
            else if (Input.GetButtonDown(player.interactButton))
            {
                player.Interact();
            }
            else if (Input.GetButtonDown(player.crouchButton))
            {
                return player.statePool.GetState("CRAWLING");
            }

            // Set animator speed and then if not moving go to Idle state
            player.anim.SetFloat("Speed", player.currentSpeed);
            if (player.directionVector.sqrMagnitude == 0)
            {
                return player.statePool.GetState("IDLE");
            }
            return this;
        }

        IPlayerState IPlayerState.ProcessMovement(PlayerMovementRB player)
        {
            if (player.controlState == ControlState.CONTROLLABLE)
            {
                // Move the player
                Vector3 velocityChange = player.directionVector - player.rb.velocity;
                velocityChange.x = Mathf.Clamp(velocityChange.x, -player.maxVelocityChange, player.maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -player.maxVelocityChange, player.maxVelocityChange);
                velocityChange.y = 0;

                player.rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }
            player.RotateToFace();
            return this;
        }

        IPlayerState IPlayerState.ProcessJump(PlayerMovementRB player)
        {
            return this;
        }
    }

}
