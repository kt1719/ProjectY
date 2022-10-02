using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class NextScene : NetworkBehaviour
{
    public int nextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        NetworkBehaviour playerNetworkBehaviour = collision.transform.root.GetComponent<NetworkBehaviour>();
        if (playerNetworkBehaviour.hasAuthority)
        {
            if (playerNetworkBehaviour.isClient)
            {
                ChangeScene(nextScene);
            }
            // Unload everything in the previous scene including this
        }
    }

    [Command(requiresAuthority = false)]
    void ChangeScene(int n)
    {
        SceneManager.LoadScene(n, LoadSceneMode.Additive);
        NetworkServer.SpawnObjects();
        ChangeSceneRPC(n);
    }

    [ClientRpc]
    void ChangeSceneRPC(int n)
    {
        if (isServer)
        {
            return;
        }
        Debug.Log("RPC Call");
        SceneManager.LoadScene(n, LoadSceneMode.Additive);
    }
}
