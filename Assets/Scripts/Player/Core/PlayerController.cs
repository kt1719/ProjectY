using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerUI;
using PlayerClasses;
using UI;
using Mirror;

namespace PlayerCore
{
    public class PlayerController : NetworkBehaviour
    {
        PlayerMovement movementscript;
        PlayerAbility abilityscript;
        PlayerClass playerClass;
        CharacterUI characterUI;

        public Camera cam;
        GameObject gameOverlay;
        
        private void Awake() // Got changed from start to awake due to the swordColl script not finding the PlayerClass script. Awake means it runs earlier than start
        {
            DontDestroyOnLoad(this.gameObject);

            characterUI = GetComponentInChildren<CharacterUI>();
            movementscript = GetComponent<PlayerMovement>();

            // TODO: make this dependent on user class choice
            abilityscript = gameObject.AddComponent<NinjaAbility>();  
            playerClass = gameObject.AddComponent<Ninja>();

            abilityscript.initialize(playerClass); // Pass in the data class into the ability class

            cam = GetComponentInChildren<Camera>();

            gameOverlay = this.transform.GetChild(2).gameObject; // The index of the heirarchy in the prefab matters

            InitializeUI();
        }
        // Update is called once per frame
        void Update()
        {
            if (!hasAuthority)
            {
                if (cam.enabled)
                {
                    cam.gameObject.SetActive(false);
                }
                if (gameOverlay.activeSelf)
                {
                    gameOverlay.SetActive(false);
                }
                return;
            }
            
            movementscript.MovePlayer();
            // movementscript.CheckDash();
            abilityscript.CheckAbility();
        }

        private void InitializeUI()
        {
            characterUI.playerClass = playerClass;
        }
    }
}
