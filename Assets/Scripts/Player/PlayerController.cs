using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerUI
{
    public class PlayerController : MonoBehaviour
    {
        PlayerMovement movementscript;
        PlayerAttack attackscript;

        private void Start()
        {
            movementscript = GetComponent<PlayerMovement>();
            attackscript = GetComponent<PlayerAttack>();
        }
        // Update is called once per frame
        void Update()
        {
            movementscript.MovePlayer();
            movementscript.CheckDash();
        }

        
    }
}
