using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.SceneManagement;

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
            GameObject player = Instantiate(playerPrefab);

            player.transform.position = UnityEngine.Random.insideUnitCircle * 7.5f; 
            player.transform.rotation = Quaternion.identity;

            // instantiating a "Player" prefab gives it the name "Player(clone)"
            // => appending the connectionId is WAY more useful for debugging!
            player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
            NetworkServer.AddPlayerForConnection(conn, player);
        }
    }

}