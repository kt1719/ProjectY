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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject player = collision.transform.root.gameObject;
        NetworkBehaviour playerNetworkBehaviour = player.GetComponent<NetworkBehaviour>();
        PlayerController playerController = player.GetComponent<PlayerController>();
        int layerNumber = CustomSceneManager.singleton.GetAvailableLayers();
        // Check if they are on the same scene as this layer can interact with all Player layers
        if (playerController.currScene != this.gameObject.scene.name || layerNumber == -1)
        {
            return;
        }
        if (playerNetworkBehaviour.hasAuthority)
        {
            if (playerNetworkBehaviour.isClient)
            {
                Debug.Log("command rpc");
                // This is to update the current scene the player controller is in as a reference for the player
                string sceneName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(nextScene));
                playerController.ChangeSceneCommand(sceneName);
                Debug.Log("curr scene is now " + playerController.currScene);
                // This is to actually load the scene
                ChangeScene(nextScene);

                //////////////////////////////////////
                // Change Player Layer
                List<string> layers = new List<string> { "Player", "PlayerEnv", "Background", "CombatLayer" };
                playerController.ChangePlayerLayer(layerNumber);
                ChangeObjectLayer(layerNumber, sceneName);
            }
        }
    }

    private static void ChangeObjectLayer(int layerNumber, string sceneName)
    {
        // Change all layers for gameobjects of that scene
        Scene s = SceneManager.GetSceneByName(sceneName);
        GameObject[] gameObjects = s.GetRootGameObjects();
        List<string> layers = new List<string> { "Background", "CombatLayer" };
        foreach (GameObject obj in gameObjects)
        {
            string layerName = (obj.name == "Enemies") ? layers[0] + layerNumber.ToString() : layers[1] + layerNumber.ToString();
            CustomSceneManager.singleton.MoveToLayer(obj.transform, LayerMask.NameToLayer(layerName));
        }
    }

    [Command(requiresAuthority = false)]
    void ChangeScene(int n)
    {
        SceneManager.LoadScene(n, LoadSceneMode.Additive);
        NetworkServer.SpawnObjects();

        // Check what layer could be used (should never be none)
        // Loop through all the gameobjects of the scene (and it's children) and change the layers of the gameobjects to use the layer mappings
        // Send TargetRPC connection to change the camera mask to only see new layer
        // Change player layer so it can only interact with new gameobject

        ChangeSceneRPC(n);
    }

    [ClientRpc]
    void ChangeSceneRPC(int n)
    {
        if (isServer)
        {
            return;
        }
        SceneManager.LoadScene(n, LoadSceneMode.Additive);
        Destroy(this.gameObject);
    }
}
