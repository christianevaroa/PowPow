using UnityEngine;
using System.Collections;

namespace PlayerScripts
{

    public class PlayerStatus : MonoBehaviour
    {
        PlayerMovementRB movement;

        // Use this for initialization
        void Start()
        {
            movement = GetComponent<PlayerMovementRB>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartRound()
        {
            movement.EnableMovement();
        }
    }

}
