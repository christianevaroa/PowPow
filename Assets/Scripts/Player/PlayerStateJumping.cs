using UnityEngine;

namespace PlayerScripts
{

    public class PlayerStateJumping : IPlayerState
    {
        string hiddenName;
        public string name { get { return hiddenName; } set { } }
        JumpTimer jumpTimer;
        Vector3 pos;

        Vector3 jumpForce = new Vector3(0f, 9f, 0f);    // 9 seems to be a little over 2x character height with double gravity

        public PlayerStateJumping()
        {
            hiddenName = "JUMPING";
            //jumpTimer = new JumpTimer(); //TODO: because JumpTimer is a MonoBehaviour I'll have to have it as a component on the player
        }

        void IPlayerState.EnterState(PlayerMovementRB player)
        {
            // TODO manually set the player's Y-position
            //player.rb.useGravity = false;
            //jumpTimer.StartTiming();
            if (player.controlState == ControlState.CONTROLLABLE)
            {
                player.anim.SetBool("Falling", true);
                player.col.material.dynamicFriction = 0f;
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
            else if(player.carryState == CarryState.CARRYING && Input.GetButtonDown(player.interactButton))
            {
                player.Interact();
            }
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
            // Not actually using this at the moment
            // TODO: do it by manually setting Y-height, but need to figure out how to also maintain X/Z momentum
            return this;
        }

    }

}