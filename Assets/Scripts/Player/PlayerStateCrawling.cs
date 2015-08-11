using UnityEngine;

namespace PlayerScripts
{
    class PlayerStateCrawling : IPlayerState
    {

        string hiddenName;

        public string name { get { return hiddenName; } set { } }

        public PlayerStateCrawling()
        {
            hiddenName = "CRAWLING";
        }

        void IPlayerState.EnterState(PlayerMovementRB player)
        {
            player.SetCrouching(true);
        }

        IPlayerState IPlayerState.Update(PlayerMovementRB player)
        {
            // Set animator speed and then if not moving go to Idle state
            player.anim.SetFloat("Speed", player.currentSpeed);

            if (player.controlState == ControlState.CONTROLLABLE)
            {
                if (player.directionVector.sqrMagnitude == 0)
                {
                    return player.statePool.GetState("CROUCHING");
                }
                if (Input.GetButtonUp(player.crouchButton))
                {
                    if (player.directionVector.sqrMagnitude == 0)
                    {
                        return player.statePool.GetState("CROUCHING");
                    }
                    else
                    {
                        return player.statePool.GetState("MOVING");
                    }
                }
            }
            else
            {
                return player.statePool.GetState("CROUCHING");
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
