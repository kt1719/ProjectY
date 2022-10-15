using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.SceneManagement;
using Spawn;
using PlayerCore;
using System.IO;
using System.Linq;

namespace Network
{
    public struct SceneAddition : NetworkMessage
    {
        public string sceneName;
        public bool notAddition;
    }

    public class CustomNetworkManager : NetworkManager
    {
        public string mainMenuScene;
        public string multiplayerScene;

        public int defaultSceneIndex = 1;
        public int defaultLayerIndex = 1;

        public void StartGame(string SceneName)
        {
            // Not used
            ServerChangeScene(SceneName);
        }

        internal void MainMenuSceneChange()
        {
            SceneManager.LoadScene(mainMenuScene);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// These are all the methods used to Start the Server
        /// </summary>

        public void StartServerV2()
        {
            if (NetworkServer.active || NetworkClient.active)
            {
                Debug.LogWarning("Server or client already started.");
                return;
            }

            mode = NetworkManagerMode.Host;

            SetupServer();
            OnStartHost();

            // scene change needed? then change scene and spawn afterwards.
            if (IsServerOnlineSceneChangeNeeded())
            {
                finishStartHostPending = true;
                ServerChangeScene(onlineScene);
            }
            // otherwise spawn directly
            else
            {
                FinishStartHost();
            }
        }

        protected void SetupServer()
        {
            // Debug.Log("NetworkManager SetupServer");
            InitializeSingleton();

            if (runInBackground)
                Application.runInBackground = true;

            if (authenticator != null)
            {
                authenticator.OnStartServer();
                authenticator.OnServerAuthenticated.AddListener(OnServerAuthenticated);
            }

            ConfigureHeadlessFrameRate();

            // start listening to network connections
            NetworkServer.Listen(maxConnections);

            // call OnStartServer AFTER Listen, so that NetworkServer.active is
            // true and we can call NetworkServer.Spawn in OnStartServer
            // overrides.
            // (useful for loading & spawning stuff from database etc.)
            //
            // note: there is no risk of someone connecting after Listen() and
            //       before OnStartServer() because this all runs in one thread
            //       and we don't start processing connects until Update.
            OnStartServer();

            // this must be after Listen(), since that registers the default message handlers
            RegisterServerMessages();
        }

        void RegisterServerMessages()
        {
            NetworkServer.OnConnectedEvent = OnServerConnectInternal;
            NetworkServer.OnDisconnectedEvent = OnServerDisconnect;
            NetworkServer.OnErrorEvent = OnServerError;
            NetworkServer.RegisterHandler<AddPlayerMessage>(OnServerAddPlayerInternal);

            // Network Server initially registers its own handler for this, so we replace it here.
            NetworkServer.ReplaceHandler<ReadyMessage>(OnServerReadyMessageInternal);
        }
        void OnServerConnectInternal(NetworkConnectionToClient conn)
        {
            //Debug.Log("NetworkManager.OnServerConnectInternal");

            if (authenticator != null)
            {
                // we have an authenticator - let it handle authentication
                authenticator.OnServerAuthenticate(conn);
            }
            else
            {
                // authenticate immediately
                OnServerAuthenticated(conn);
            }
        }

        void OnServerAuthenticated(NetworkConnectionToClient conn)
        {
            //Debug.Log("NetworkManager.OnServerAuthenticated");

            // set connection to authenticated
            conn.isAuthenticated = true;

            // proceed with the login handshake by calling OnServerConnect
            if (networkSceneName != "" && networkSceneName != offlineScene)
            {
                //SceneMessage msg = new SceneMessage() { sceneName = networkSceneName };
                //conn.Send(msg);
                if (CustomSceneManager.singleton && NetworkServer.active)
                {
                    string sceneName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(defaultSceneIndex));
                    SceneAddition msg = new SceneAddition() { sceneName = sceneName, notAddition = true };
                    conn.Send(msg);
                    CustomSceneManager sceneManagerInstance = CustomSceneManager.singleton.GetComponent<CustomSceneManager>();
                    foreach (var item in sceneManagerInstance.ReturnLoadedScenes())
                    {
                        if (item == defaultSceneIndex)
                        {
                            continue;
                        }
                        sceneName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(item));
                        conn.Send(new SceneAddition { sceneName = sceneName });
                    }
                }
            }

            OnServerConnect(conn);
        }

        /// <summary>
        /// End of start server functions
        /// </summary>
        /////////////////////////////////////////////////////////////////////////////////

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            // Implement logic here for custom instantiation
            SpawnPoint spawnPointScript = SceneManager.GetActiveScene().GetRootGameObjects().Where(x => x.name == "Spawn_Point").ToArray()[0].GetComponent<SpawnPoint>();
            GameObject player = Instantiate(playerPrefab);

            player.transform.position = spawnPointScript.ReturnCenterPos();
            player.transform.rotation = Quaternion.identity;

            // Could add in the future to automatically input the scene they've saved at
            player.GetComponent<PlayerController>().currScene = SceneUtility.GetBuildIndexByScenePath(this.onlineScene);

            // instantiating a "Player" prefab gives it the name "Player(clone)"
            // => appending the connectionId is WAY more useful for debugging!
            player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
            NetworkServer.AddPlayerForConnection(conn, player);
            NetworkClient.isLoadingScene = false;
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (!NetworkClient.isHostClient)
            {
                NetworkClient.RegisterHandler<SceneAddition>(ClientLoadAdditionalScenes);
            }
        }

        void ClientLoadAdditionalScenes(SceneAddition msg)
        {
            if (NetworkClient.isConnected)
            {
                if (msg.notAddition)
                {
                    loadingSceneAsync = SceneManager.LoadSceneAsync(msg.sceneName, LoadSceneMode.Single);
                }
                else
                {
                    loadingSceneAsync = SceneManager.LoadSceneAsync(msg.sceneName, LoadSceneMode.Additive);
                }
            }
        }

        public override void OnStartServer()
        {
            // Most likely add the scene thing here?
            base.OnStartServer();
        }
    }

}