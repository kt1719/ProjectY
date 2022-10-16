using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using PlayerCore;
using System.IO;
using UI;

public class NextScene : NetworkBehaviour
{
    public int nextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject player = collision.transform.root.gameObject;
        NetworkBehaviour playerNetworkBehaviour = player.GetComponent<NetworkBehaviour>();
        PlayerController playerController = player.GetComponent<PlayerController>();
        // Check if they are on the same scene as this layer can interact with all Player layers
        if (playerController.currScene != this.gameObject.scene.buildIndex) 
        {
            return;
        }
        if (playerNetworkBehaviour.hasAuthority)
        {
            CharacterUI.instance.StartTransitionAnimation();
            playerController.FreezeCharacter();
            //////////////////////////////////////
            playerController.ChangeScene(nextScene);
            if (NetworkClient.active && !isServer)
            {
                playerController.ChangeSceneCommand(nextScene);
            }

            /////////////////////////////////////////

            int currentAvailableLayer = CustomSceneManager.singleton.CheckSceneAlreadyLoaded(nextScene);
            if (currentAvailableLayer == -1)
            {
                // If it is not -1 then it is already loaded and we do not need to try assigning it a new layer
                currentAvailableLayer = CustomSceneManager.singleton.GetAvailableLayers();
                if (currentAvailableLayer == -1)
                {
                    Debug.LogError("Ran out of layers");
                    return;
                }
                // This is to actually load the scene
                CustomSceneManager.singleton.ServerChangeSceneV2(nextScene, currentAvailableLayer);
            }
            else
            {
                CharacterUI.instance.SetReadyForTransition();
            }
            playerController.ChangePlayerLayerCommand(currentAvailableLayer);
        }
    }
}
