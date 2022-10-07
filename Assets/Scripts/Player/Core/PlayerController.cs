using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnim;
using PlayerClasses;
using UI;
using Mirror;
using System.IO;

namespace PlayerCore
{
    public class PlayerController : NetworkBehaviour
    {
        PlayerMovement movementscript;
        PlayerAbility abilityscript;
        PlayerClass playerClass;
        PlayerAnimation animationScript;

        Camera[] cameras;
        GameObject gameOverlay;

        SpriteRenderer spriteRenderer;

        public bool singlePlayer = false;
        public string currScene;

        private void Awake() // Got changed from start to awake due to the swordColl script not finding the PlayerClass script. Awake means it runs earlier than start
        {
            DontDestroyOnLoad(this.gameObject);
            // TODO: make this dependent on user class choice
            abilityscript = GetComponent<WarriorAbility>();  
            playerClass = GetComponent<Warrior>();
            movementscript = GetComponent<WarriorMovement>();
            animationScript = GetComponent<PlayerAnimation>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            abilityscript.initialize(playerClass); // Pass in the data class into the ability class

            cameras = GetComponentsInChildren<Camera>();

            gameOverlay = this.transform.GetChild(2).gameObject; // The index of the heirarchy in the prefab matters
        }

        private void Start()
        {
            SetupVariables();
        }

        private void SetupVariables()
        {
            // Could add in the future to automatically input the scene they've saved at
            if (hasAuthority)
            {
                ChangeSceneCommand("");
            }

            if (hasAuthority || singlePlayer)
            {
                spriteRenderer.enabled = false;
                FreezeCharacter();
            }
            else
            {
                // Skips the player setup animations
                animationScript.SetPlayerNonLocal();
                foreach (Transform child in transform)
                {
                    // Set all child gameobjects of 
                    if (child.name == "SwordColliders" || child.name == "EnvironmentBody")
                    {
                        continue;
                    }
                    child.gameObject.SetActive(false);
                }
            }
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

        // Ask the server what scene am I meant to be spawned in
        [Command]
        public void ChangeSceneCommand(string scene)
        {
            if (scene == "")
            {
                ChangeSceneRPC(this.currScene);
            }
            else
            {
                ChangeSceneRPC(scene);
            }
        }

        [TargetRpc]
        public void ChangeSceneRPC(string scene)
        {
            if (isServer)
            {
                return;
            }
            currScene = scene;
        }

        [TargetRpc]
        public void ChangePlayerLayer(int layerNum)
        {
            // Change actual layer of objects
            List<string> layers = new List<string> { "Player", "PlayerEnv", "CombatLayer" };
            this.gameObject.layer = LayerMask.NameToLayer(layers[0] + layerNum.ToString());
            foreach (Transform obj in this.transform)
            {
                if (obj.name == "BackgroundCanvas")
                {
                    continue;
                }
                string layerName = (obj.name == "SwordColliders") ? layers[2] + layerNum.ToString() : (obj.name == "EnvironmentBody") ? layers[1] + layerNum.ToString() : layers[0] + layerNum.ToString();
                CustomSceneManager.singleton.MoveToLayer(obj, LayerMask.NameToLayer(layerName));
            }

            // Change camera masks
        }

        //////////////////////////////////////////////////////

        public void SpawnPlayerAnimation()
        {
            // This is because sprite renderers can have different behaviours on multiplayer
            spriteRenderer.enabled = true;
            animationScript.SpawnPlayer();
        }

        [ClientRpc]
        public void SpawnPlayerAnimationRPC()
        {
            SpawnPlayerAnimation();
        }

        [Command]
        public void SpawnPlayerAnimationCommand()
        {
            SpawnPlayerAnimationRPC();
        }

        //////////////////////////////////////////////////////

        [ClientRpc (includeOwner = false)]
        public void TurnOffRenderer()
        {
            spriteRenderer.enabled = false;
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
