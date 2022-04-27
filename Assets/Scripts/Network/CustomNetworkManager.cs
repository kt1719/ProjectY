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
    }

}