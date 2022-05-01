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
            Transform startpos = new GameObject().transform;
            startpos.position = UnityEngine.Random.insideUnitCircle * 7.5f; 
            startpos.rotation = Quaternion.identity;

            GameObject player = Instantiate(playerPrefab, startpos.position, startpos.rotation);

            // instantiating a "Player" prefab gives it the name "Player(clone)"
            // => appending the connectionId is WAY more useful for debugging!
            player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
            NetworkServer.AddPlayerForConnection(conn, player);
        }
    }

}