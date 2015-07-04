using UnityEngine;
using System.Collections;

namespace PlayerScripts
{

    public class PlayerStatus : MonoBehaviour
    {
        //PlayerMovement movement;
        PlayerMovementRB movement;

        // Use this for initialization
        void Start()
        {
            //movement = GetComponent<PlayerMovement>();
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
