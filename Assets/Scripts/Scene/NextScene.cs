using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using PlayerCore;
using System.IO;

public class NextScene : NetworkBehaviour
{
    public int nextScene;
    int currentAvailableLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject player = collision.transform.root.gameObject;
        NetworkBehaviour playerNetworkBehaviour = player.GetComponent<NetworkBehaviour>();
        PlayerController playerController = player.GetComponent<PlayerController>();
        currentAvailableLayer = CustomSceneManager.singleton.GetAvailableLayers();
        // Check if they are on the same scene as this layer can interact with all Player layers
        if (playerController.currScene != this.gameObject.scene.buildIndex || currentAvailableLayer == -1)
        {
            return;
        }
        if (playerNetworkBehaviour.hasAuthority)
        {
            string sceneName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(nextScene));
            // This is to actually load the scene
            CustomSceneManager.singleton.ServerChangeSceneV2(sceneName, currentAvailableLayer);
            //////////////////////////////////////
            playerController.ChangeSceneCommand(nextScene);
            playerController.UpdateScenePosition(nextScene);
            playerController.ChangePlayerLayer(currentAvailableLayer);
        }
    }
}
