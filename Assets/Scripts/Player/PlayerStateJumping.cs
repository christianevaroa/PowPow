using UnityEngine;

namespace PlayerScripts
{

    public class PlayerStateJumping : IPlayerState
    {
        string hiddenName;
        public string name { get { return hiddenName; } set { } }
        JumpTimer jumpTimer;
        Vector3 pos;

        Vector3 jumpForce = new Vector3(0f, 5f, 0f);

        public PlayerStateJumping()
        {
            hiddenName = "JUMPING";
            jumpTimer = new JumpTimer();
        }

        void IPlayerState.EnterState(PlayerMovementRB player)
        {
            Vector3 pos = player.rb.position;
            // TODO manually set the player's Y-position
            //player.rb.useGravity = false;
            //jumpTimer.StartTiming();
            if (player.controlState == PlayerMovementRB.ControlState.CONTROLLABLE)
            {
                player.rb.AddForce(jumpForce, ForceMode.VelocityChange);
            }
        }

        IPlayerState IPlayerState.Update(PlayerMovementRB player)
        {
            if (player.rb.velocity.y < 0)
            {
                return player.statePool.GetState("FALLING");
            }
            else if (player.grounded)
            {
                return player.statePool.GetState("IDLE");
            }
            return this;
        }

        IPlayerState IPlayerState.ProcessMovement(PlayerMovementRB player)
        {
            return this;
        }

        IPlayerState IPlayerState.ProcessJump(PlayerMovementRB player)
        {
            // Not actually using this at the moment
            // TODO: do it by manually setting Y-height, but need to figure out how to also maintain X/Z momentum
            return this;
        }

    }

}