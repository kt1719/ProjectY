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
        PlayerAbility abilityscript;
        PlayerClass playerClass;
        
        // TODO: imo we don't need a controller class, like we just need to call the below Update stuff in each scripts' Update() function
        private void Start()
        {
            movementscript = GetComponent<PlayerMovement>();

            // TODO: make this dependent on user class choice
            abilityscript = gameObject.AddComponent<WarriorAbility>();  
            playerClass = gameObject.AddComponent<Warrior>();

            abilityscript.initialize(playerClass); // Pass in the data class into the ability class

        }
        // Update is called once per frame
        void Update()
        {
            movementscript.MovePlayer();
            movementscript.CheckDash();
            abilityscript.CheckAbility();
        }

        
    }
}
