using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnim;
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
        PlayerAnimation animationScript;
        CharacterUI characterUI;

        Camera[] cameras;
        GameObject gameOverlay;

        public bool singlePlayer = false;

        private void Awake() // Got changed from start to awake due to the swordColl script not finding the PlayerClass script. Awake means it runs earlier than start
        {
            DontDestroyOnLoad(this.gameObject);

            characterUI = GetComponentInChildren<CharacterUI>();

            // TODO: make this dependent on user class choice
            abilityscript = GetComponent<WarriorAbility>();  
            playerClass = GetComponent<Warrior>();
            movementscript = GetComponent<WarriorMovement>();
            animationScript = GetComponent<PlayerAnimation>();

            abilityscript.initialize(playerClass); // Pass in the data class into the ability class

            cameras = GetComponentsInChildren<Camera>();

            gameOverlay = this.transform.GetChild(2).gameObject; // The index of the heirarchy in the prefab matters

            InitializeUI();
        }
        // Update is called once per frame
        void Update()
        {
            if (!hasAuthority && !singlePlayer)
            {
                foreach (Camera cam in cameras)
                {
                    cam.gameObject.SetActive(hasAuthority || singlePlayer);
                }
                if (gameOverlay.activeSelf)
                {
                    gameOverlay.SetActive(false);
                }
                return;
            }
            abilityscript.CheckAbility();
            movementscript.ChangeAnimatorAnimation();
        }

        void LateUpdate()
        {
            if (!hasAuthority && !singlePlayer)
            {
                return;
            }
            movementscript.FlipMovement();
        }

        void FixedUpdate()
        {
            if (!hasAuthority && !singlePlayer)
            {
                return;
            }
            // Moved to fixed update. Better for rigidbodies + colliders etc
            movementscript.MovePlayer();
        }

        private void InitializeUI()
        {
            characterUI.playerClass = playerClass;
        }

        public void SpawnPlayer() 
        {
            GetComponent<SpriteRenderer>().enabled = true;
            animationScript.SpawnPlayer();
        }

        public void FreezeCharacter()
        {
            movementscript.FreezeMovement();
        }

        public void UnFreezeCharacter()
        {
            movementscript.UnFreezeMovement();
        }
    }
}
