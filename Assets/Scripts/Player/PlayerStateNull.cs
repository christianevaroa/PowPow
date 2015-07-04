using UnityEngine;

namespace PlayerScripts
{
    public class PlayerStateNull : IPlayerState
    {
        string hiddenName;
        public string name { get { return hiddenName; } set { } }

        public PlayerStateNull()
        {
            hiddenName = "NULL";
        }

        void IPlayerState.EnterState(PlayerMovementRB player)
        {
            Error();
        }

        IPlayerState IPlayerState.Update(PlayerMovementRB player)
        {
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

        IPlayerState Error()
        {
            Debug.LogError("Player is in invalid state PlayerStateNull");
            return this;
        }
    }
}
