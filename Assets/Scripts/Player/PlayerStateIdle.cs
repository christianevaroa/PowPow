using UnityEngine;

namespace PlayerScripts
{

    public class PlayerStateIdle : IPlayerState
    {

        string hiddenName;
        public string name { get { return hiddenName; } set { } }

        public PlayerStateIdle()
        {
            hiddenName = "IDLE";
        }

        void IPlayerState.EnterState(PlayerMovementRB player)
        {
            player.SetCrouching(false);
            player.anim.SetFloat("Speed", 0f);
        }

        IPlayerState IPlayerState.Update(PlayerMovementRB player)
        {
            if (player.controlState == ControlState.CONTROLLABLE)
            {
                if (player.directionVector.sqrMagnitude != 0)
                {
                    return player.statePool.GetState("MOVING");
                }
                else if (Input.GetButtonDown(player.jumpButton))
                {
                    return player.statePool.GetState("JUMPING");
                }
                else if (Input.GetButtonDown(player.interactButton))
                {
                    player.Interact();
                }
                else if (Input.GetButtonDown(player.crouchButton))
                {
                    return player.statePool.GetState("CROUCHING");
                }
            }

            if (!player.grounded)
            {
                return player.statePool.GetState("FALLING");
            }

            // TODO: if( crouch is pressed) { return new PlayerStateCrouching(); }
            return this;
        }

        IPlayerState IPlayerState.ProcessMovement(PlayerMovementRB player)
        {
            if (player.controlState == ControlState.CONTROLLABLE)
            {
                player.RotateToFace();
            }
            return this;
        }

        IPlayerState IPlayerState.ProcessJump(PlayerMovementRB player)
        {
            return this;
        }
    }

}
