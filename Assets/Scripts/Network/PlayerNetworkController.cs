using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Network
{
    public class PlayerNetworkController : NetworkBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        public void CanStartGame(string SceneName)
        {
            if (hasAuthority)
            {
                CmdCanStartGame(SceneName);
            }
        }
        
        [Command]
        public void CmdCanStartGame(string SceneName)
        {
            
        }
    }
}
