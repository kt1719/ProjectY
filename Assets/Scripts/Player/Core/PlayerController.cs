using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnim;
using PlayerClasses;
using UI;
using Mirror;
using System.IO;
using Spawn;
using UnityEngine.SceneManagement;
using System.Linq;
using Network;
using PlayerCam;

namespace PlayerCore
{
    public class PlayerController : NetworkBehaviour
    {
        PlayerMovement movementscript;
        PlayerAbility abilityscript;
        PlayerClass playerClass;
        PlayerAnimation animationScript;

        Camera[] cameras;
        Camera mainCam;
        GameObject gameOverlay;

        SpriteRenderer spriteRenderer;

        public bool singlePlayer = false;
        public int currScene = -1;
        public int prevScene = -1;
        [SyncVar]
        public int currentLayerNumber = 1;
        [SyncVar]
        public bool spawned = false;

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
            mainCam = this.transform.Find("Main Camera").GetComponent<Camera>();

            gameOverlay = this.transform.GetChild(2).gameObject; // The index of the heirarchy in the prefab matters
        }

        private void Start()
        {
            SetupVariables();
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

        private void SetupVariables()
        {
            // Could add in the future to automatically input the scene they've saved at
            if (!spawned)
            {
                spriteRenderer.enabled = false;
                FreezeCharacter();
            }
            if (hasAuthority)
            {
                ChangeSceneCommand(-1);

                // TODO: Have to check what layer the scene is
                currentLayerNumber = CustomNetworkManager.singleton.GetComponent<CustomNetworkManager>().defaultLayerIndex;
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
                ChangePlayerLayerLocal(currentLayerNumber);
            }

            if (NetworkClient.localPlayer.netId != this.netId)
            {
                GameObject spawnPoint = SceneManager.GetActiveScene().GetRootGameObjects().Where(x => x.name == "Spawn_Point").ToArray()[0]; // Find spawnpoint gameobject
                SpawnPoint spawnPointScript = spawnPoint.GetComponent<SpawnPoint>();
                spawnPointScript.playerSpawnQueue.Enqueue(this.gameObject);
            }
        }

        // Ask the server what scene am I meant to be spawned in
        [Command]
        public void ChangeSceneCommand(int scene)
        {
            if (scene == -1)
            {
                ChangeSceneRPC(this.currScene);
            }
            else
            {
                // When scene change is provided by client, change it on server
                ChangeScene(scene);
            }
            CustomSceneManager.singleton.GetComponent<CustomSceneManager>().IncrementScenePlayerCount(prevScene, currScene);
        }

        [TargetRpc]
        public void ChangeSceneRPC(int scene)
        {
            ChangeScene(scene);
        }

        public void ChangeScene(int scene)
        {
            prevScene = currScene;
            currScene = scene;
        }

        [Command]
        public void ChangePlayerLayerCommand(int layerNum)
        {
            ChangePlayerLayerRPC(layerNum);
            SetReadySceneTransition(layerNum);
            currentLayerNumber = layerNum;
        }

        [ClientRpc(includeOwner = false)]
        public void ChangePlayerLayerRPC(int layerNum)
        {
            ChangePlayerLayerLocal(layerNum);
        }

        [TargetRpc]
        public void SetReadySceneTransition(int layerNum)
        {
            currentLayerNumber = layerNum;
            CharacterUI.instance.SetReadyForTransition();
        }

        public void ChangePlayerLayerLocal(int layerNum = -1)
        {
            layerNum = (layerNum == -1) ? currentLayerNumber : layerNum;
            ChangeChildObjectLayers(layerNum);

            void ChangeChildObjectLayers(int layerNum)
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
                    CustomSceneManager.MoveToLayer(obj, LayerMask.NameToLayer(layerName));
                }
            }
        }

        public void UpdateCameraLayer()
        {
            if (!this.hasAuthority) { return; }

            List<string> cameraMaskLayers = new List<string> { "Player", "CombatLayer", "Background" };

            ChangeCameraLayer(currentLayerNumber, cameraMaskLayers);

            void ChangeCameraLayer(int layerNum, List<string> cameraMaskLayers)
            {
                // Reset camera culling mask
                mainCam.cullingMask = 0;
                // Change camera culling mask
                foreach (string s in cameraMaskLayers)
                {
                    string maskName = s + layerNum.ToString();
                    // https://answers.unity.com/questions/348974/edit-camera-culling-mask.html
                    mainCam.cullingMask |= (1 << LayerMask.NameToLayer(maskName));
                }
            }
        }

        public void UpdatePlayerScenePosition()
        {
            movementscript.UpdateScenePosition();
        }

        public void UpdateCameraBounds()
        {
            mainCam.GetComponent<CameraBounds>().UpdateCameraBounds();
        }

        //////////////////////////////////////////////////////

        public void SpawnPlayerAnimation()
        {
            // This is because sprite renderers can have different behaviours on multiplayer
            spriteRenderer.enabled = true;
            animationScript.SpawnPlayer();
            if (hasAuthority)
            {
                UpdateSpawnedServer();
            }
        }

        [Command]
        public void UpdateSpawnedServer()
        {
            spawned = true;
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
