using UnityEngine;

namespace PlayerScripts
{
    public class PlayerStateFalling : IPlayerState
    {

        string hiddenName;
        public string name { get { return hiddenName; } set { } }

        public PlayerStateFalling()
        {
            hiddenName = "FALLING";
        }

        void IPlayerState.EnterState(PlayerMovementRB player)
        {
            player.anim.SetBool("Falling", true);
            // Minimum friction so you don't stick to walls while falling
            player.col.material.dynamicFriction = 0f;
        }

        IPlayerState IPlayerState.Update(PlayerMovementRB player)
        {
            if (player.grounded)
            {
                player.anim.SetBool("Falling", false);

                // Reset friction before leaving this state
                player.col.material.dynamicFriction = 1f;

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
            return this;
        }
    }
}
