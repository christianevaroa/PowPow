using UnityEngine;
using System.Collections.Generic;

namespace PlayerScripts
{
    /// <summary>
    /// Used to pool PlayerState objects so they aren't constantly being created and garbage collected.
    /// Each player character will have one of these, each with it's own Dictionary of states.
    /// 
    /// Keys in the dictionary are in ALL CAPS
    /// </summary>
    public class PlayerStatePool
    {
        Dictionary<string, IPlayerState> states;

        public PlayerStatePool()
        {
            states = new Dictionary<string, IPlayerState>();
            PopulateDict();
        }

        void PopulateDict()
        {
            states.Add("NULL", new PlayerStateNull());
            states.Add("IDLE", new PlayerStateIdle());
            states.Add("MOVING", new PlayerStateMoving());
            states.Add("JUMPING", new PlayerStateJumping());
            states.Add("FALLING", new PlayerStateFalling());
            //TODO: implement these
            //states.Add("CROUCHING", new PlayerStateCrouching());
            //states.Add("CRAWLING", new PlayerStateCrawling());
        }

        /// <summary>
        /// Request a state from the pool.
        /// </summary>
        /// <param name="statename">The state type</param>
        /// <returns>The appropriate state from the pool, or the null state if something goes wrong</returns>
        public IPlayerState GetState(string statename)
        {
            IPlayerState ret;
            if (states.TryGetValue(statename, out ret))
            {
                return ret;
            }
            Debug.LogError("Tried to look up an invalid key in the PlayerStatePool - " + statename + " (state names should be all caps remember!)");
            return states["NULL"];
        }
    }
}
