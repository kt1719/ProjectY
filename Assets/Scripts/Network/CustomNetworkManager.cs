using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Network
{
    public class CustomNetworkManager : NetworkManager
    {
        public void StartGame(string SceneName)
        {
            ServerChangeScene(SceneName);
        }
    }

}