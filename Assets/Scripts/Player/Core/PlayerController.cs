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
        
        private void Awake() // Got changed from start to awake due to the swordColl script not finding the PlayerClass script. Awake means it runs earlier than start
        {
            characterUI = GameObject.Find("GameOverlay").GetComponent<CharacterUI>(); // Not sure if this will work in multiplayer setting. Never actually done full multiplayer so need to do more reasearch

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
