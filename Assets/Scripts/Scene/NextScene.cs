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
    int intCurrentAvailableLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject player = collision.transform.root.gameObject;
        NetworkBehaviour playerNetworkBehaviour = player.GetComponent<NetworkBehaviour>();
        PlayerController playerController = player.GetComponent<PlayerController>();
        intCurrentAvailableLayer = CustomSceneManager.singleton.GetAvailableLayers();
        // Check if they are on the same scene as this layer can interact with all Player layers
        if (playerController.currScene != this.gameObject.scene.name || intCurrentAvailableLayer == -1)
        {
            return;
        }
        if (playerNetworkBehaviour.hasAuthority)
        {
            if (playerNetworkBehaviour.isClient)
            {
                string sceneName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(nextScene));
                // This is to actually load the scene
                ChangeScene(nextScene, intCurrentAvailableLayer);

                //////////////////////////////////////
                playerController.ChangeSceneCommand(sceneName);
                // Change Player Layer
                playerController.ChangePlayerLayer(intCurrentAvailableLayer);
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = Path.GetFileNameWithoutExtension(scene.name);
        ChangeObjectLayer(intCurrentAvailableLayer, sceneName);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private static void ChangeObjectLayer(int layerNumber, string sceneName)
    {
        // Change all layers for gameobjects of that scene
        Scene s = SceneManager.GetSceneByName(sceneName);
        GameObject[] gameObjects = s.GetRootGameObjects();
        List<string> layers = new List<string> { "Background", "CombatLayer" };
        foreach (GameObject obj in gameObjects)
        {
            string layerName = (obj.name != "Enemies") ? layers[0] + layerNumber.ToString() : layers[1] + layerNumber.ToString();
            CustomSceneManager.singleton.MoveToLayer(obj.transform, LayerMask.NameToLayer(layerName));
        }
    }

    [Command(requiresAuthority = false)]
    void ChangeScene(int n, int currAvailableLayer)
    {
        intCurrentAvailableLayer = currAvailableLayer;
        SceneManager.LoadScene(n, LoadSceneMode.Additive);
        string sceneName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(n));
        ChangeObjectLayer(intCurrentAvailableLayer, sceneName);
        // NetworkServer.SpawnObjects();

        // Check what layer could be used (should never be none)
        // Loop through all the gameobjects of the scene (and it's children) and change the layers of the gameobjects to use the layer mappings
        // Send TargetRPC connection to change the camera mask to only see new layer
        // Change player layer so it can only interact with new gameobject

        ChangeSceneRPC(n, currAvailableLayer);
    }

    [ClientRpc]
    void ChangeSceneRPC(int n, int currAvailableLayer)
    {
        Debug.Log("RPC sent");
        if (isServer)
        {
            return;
        }
        intCurrentAvailableLayer = currAvailableLayer;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(n, LoadSceneMode.Additive);
    }
}
