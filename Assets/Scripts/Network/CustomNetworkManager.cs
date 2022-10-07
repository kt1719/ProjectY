using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.SceneManagement;
using Spawn;
using PlayerCore;
using System.IO;

namespace Network
{
    public class CustomNetworkManager : NetworkManager
    {
        public string mainMenuScene;
        public string multiplayerScene;
        public void StartGame(string SceneName)
        {
            // Not used
            ServerChangeScene(SceneName);
        }

        internal void MainMenuSceneChange()
        {
            SceneManager.LoadScene(mainMenuScene);
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            // Implement logic here for custom instantiation
            SpawnPoint spawnPointScript = GameObject.Find("Spawn_Point").GetComponent<SpawnPoint>();
            GameObject player = Instantiate(playerPrefab);

            player.transform.position = spawnPointScript.ReturnCenterPos();
            player.transform.rotation = Quaternion.identity;

            // Could add in the future to automatically input the scene they've saved at
            player.GetComponent<PlayerController>().currScene = Path.GetFileNameWithoutExtension(this.onlineScene);

            // instantiating a "Player" prefab gives it the name "Player(clone)"
            // => appending the connectionId is WAY more useful for debugging!
            player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
            NetworkServer.AddPlayerForConnection(conn, player);
            player.GetComponent<PlayerController>().TurnOffRenderer();
        }

        public override void OnStartServer()
        {
            base.OnStartServer();


        }
    }

}