using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System.IO;

public struct LayerMessage : NetworkMessage
{
    public int layerNumber;
    public string sceneName;
}

public class CustomSceneManager : NetworkBehaviour
{
    public static CustomSceneManager singleton;
    Dictionary<string, int> sceneToLayerMapping;
    HashSet<int> layersAvailable = new HashSet<int> { 1, 2, 3, 4, 5 };
    int availableServerLayer;

    public void Start()
    {
        if (!NetworkClient.active) { return; }

        NetworkClient.RegisterHandler<LayerMessage>(LayerMessageCallback);
        Object.DontDestroyOnLoad(this);
        singleton = this;
    }

    private void LayerMessageCallback(LayerMessage msg)
    {
        ChangeObjectLayer(msg.layerNumber, msg.sceneName);
    }

    // Used to get available scene layers that can be used when loading the scene
    public int GetAvailableLayers()
    {
        return 2;
    }

    // A function used to recursively change all the gameobject layers upon loading the scene
    private void ChangeObjectLayer(int layerNumber, string sceneName)
    {
        // Change all layers for gameobjects of that scene
        Scene s = SceneManager.GetSceneByName(sceneName);
        GameObject[] gameObjects = s.GetRootGameObjects();
        List<string> layers = new List<string> { "Background", "CombatLayer" };
        foreach (GameObject obj in gameObjects)
        {
            string layerName = (obj.name != "Enemies") ? layers[0] + layerNumber.ToString() : layers[1] + layerNumber.ToString();
            MoveToLayer(obj.transform, LayerMask.NameToLayer(layerName));
        }
    }

    public void MoveToLayer(Transform root, int layer)
    {
        root.gameObject.layer = layer;
        foreach (Transform child in root)
        {
            MoveToLayer(child, layer);
        }
    }

    // Used to load the scene as an addition for both the server and clients
    [Command(requiresAuthority = false)]
    public void ServerChangeSceneV2(string sceneName, int layerNumber)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogError("ServerChangeScene empty scene name");
            return;
        }

        if (NetworkServer.isLoadingScene)
        {
            Debug.LogError($"Scene change is already in progress for {sceneName}");
            return;
        }

        Debug.Log($"ServerChangeScene {sceneName}");
        // NetworkServer.SetAllClientsNotReady();

        // Let server prepare for scene change
        NetworkManager.singleton.OnServerChangeScene(sceneName);

        // set server flag to stop processing messages while changing scenes
        // it will be re-enabled in FinishLoadScene.
        NetworkServer.isLoadingScene = true;

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        availableServerLayer = layerNumber;
        SceneManager.sceneLoaded += ServerLevelLoaded;

        // ServerChangeScene can be called when stopping the server
        // when this happens the server is not active so does not need to tell clients about the change
        if (NetworkServer.active)
        {
            // notify all clients about the new scene
            NetworkServer.SendToAll(new SceneMessage { sceneName = sceneName, sceneOperation = SceneOperation.LoadAdditive });
            NetworkServer.SendToAll(new LayerMessage { sceneName = sceneName, layerNumber = layerNumber });
        }

        NetworkServer.isLoadingScene = false;
    }

    private void ServerLevelLoaded(Scene scene, LoadSceneMode arg1)
    {
        ChangeObjectLayer(availableServerLayer, Path.GetFileNameWithoutExtension(scene.name));
        SceneManager.sceneLoaded -= ServerLevelLoaded;
    }
}
