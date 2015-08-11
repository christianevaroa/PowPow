using UnityEngine;

namespace PlayerScripts
{
    class PlayerStateCrouching : IPlayerState
    {

        string hiddenName;
        public string name { get { return hiddenName; } set { } }

        public PlayerStateCrouching()
        {
            hiddenName = "CROUCHING";
        }

        void IPlayerState.EnterState(PlayerMovementRB player)
        {
            player.anim.SetFloat("Speed", 0f);
            player.SetCrouching(true);
        }

        IPlayerState IPlayerState.Update(PlayerMovementRB player)
        {
            if (player.controlState == ControlState.CONTROLLABLE)
            {
                if (!Input.GetButton(player.crouchButton))
                {
                    return player.statePool.GetState("IDLE");
                }
                if (player.directionVector.sqrMagnitude != 0)
                {
                    return player.statePool.GetState("CRAWLING");
                }
            }

            if (!player.grounded)
            {
                return player.statePool.GetState("FALLING");
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
            return this;
        }
    }
}
