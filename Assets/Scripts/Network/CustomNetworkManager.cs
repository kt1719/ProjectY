using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.SceneManagement;
using Spawn;
using PlayerCore;

namespace Network
{
    public class CustomNetworkManager : NetworkManager
    {
        public string mainMenuScene;
        public string multiplayerScene;
        public void StartGame(string SceneName)
        {
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

            // instantiating a "Player" prefab gives it the name "Player(clone)"
            // => appending the connectionId is WAY more useful for debugging!
            player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
            NetworkServer.AddPlayerForConnection(conn, player);
            player.GetComponent<PlayerController>().TurnOffRenderer();
        }
    }

}