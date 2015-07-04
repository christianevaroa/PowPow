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
            player.anim.SetFloat("Speed", 0f);
        }

        IPlayerState IPlayerState.Update(PlayerMovementRB player)
        {
            
            if (player.controlState == PlayerMovementRB.ControlState.CONTROLLABLE && player.directionVector.sqrMagnitude != 0)
            {
                return player.statePool.GetState("MOVING");
            }
            else if (Input.GetButtonDown(player.jumpButton))
            {
                return player.statePool.GetState("JUMPING");
            }
            else if (!player.grounded)
            {
                return player.statePool.GetState("FALLING");
            }

            // TODO: if( crouch is pressed) { return new PlayerStateCrouching(); }
            return this;
        }

        IPlayerState IPlayerState.ProcessMovement(PlayerMovementRB player)
        {
            player.RotateToFace();
            return this;
        }

        IPlayerState IPlayerState.ProcessJump(PlayerMovementRB player)
        {
            return this;
        }
    }

}
