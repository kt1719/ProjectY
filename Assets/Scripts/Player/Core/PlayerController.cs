using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAtt;
using PlayerUI;
using PlayerClasses;

namespace PlayerCore
{
    public class PlayerController : MonoBehaviour
    {
        PlayerMovement movementscript;
        PlayerAttack attackscript;
        PlayerClass playerClass;
        

        private void Start()
        {
            movementscript = GetComponent<PlayerMovement>();
            attackscript = GetComponent<PlayerAttack>();

            // TODO: make this dependent on user class choice
            gameObject.AddComponent<WarriorAbility>();  
            playerClass = gameObject.AddComponent<Warrior>();
        }
        // Update is called once per frame
        void Update()
        {
            movementscript.MovePlayer();
            movementscript.CheckDash();
            playerClass.CheckAbility();
        }

        
    }
}
