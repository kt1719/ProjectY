using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerUI;
using PlayerClasses;
using UI;

namespace PlayerCore
{
    public class PlayerController : MonoBehaviour
    {
        PlayerMovement movementscript;
        PlayerAbility abilityscript;
        PlayerClass playerClass;

        CharacterUI characterUI;
        
        private void Start()
        {
            characterUI = GameObject.Find("GameOverlay").GetComponent<CharacterUI>();

            movementscript = GetComponent<PlayerMovement>();

            // TODO: make this dependent on user class choice
            abilityscript = gameObject.AddComponent<WarriorAbility>();  
            playerClass = gameObject.AddComponent<Warrior>();

            abilityscript.initialize(playerClass); // Pass in the data class into the ability class

            InitializeUI();
        }
        // Update is called once per frame
        void Update()
        {
            movementscript.MovePlayer();
            movementscript.CheckDash();
            abilityscript.CheckAbility();
        }

        private void InitializeUI()
        {
            characterUI.playerClass = playerClass;
        }
    }
}
