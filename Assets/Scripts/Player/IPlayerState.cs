using UnityEngine;

namespace PlayerScripts {

    public interface IPlayerState
    {
        string name { get; }
        void EnterState(PlayerMovementRB player);
        IPlayerState Update(PlayerMovementRB player);
        IPlayerState ProcessMovement(PlayerMovementRB player);
        IPlayerState ProcessJump(PlayerMovementRB player);

    }

}

