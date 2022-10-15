using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using Network;

public struct LayerMessage : NetworkMessage
{
    public int layerNumber;
    public string sceneName;
}

public class CustomSceneManager : NetworkBehaviour
{
    public static CustomSceneManager singleton;
    SyncDictionary<int, int> sceneToLayerMapping = new SyncDictionary<int, int>();
    SyncDictionary<int, int> scenePlayerCount = new SyncDictionary<int, int>(); // Mapping of scene id to player count
    SyncHashSet<int> layersAvailable = new SyncHashSet<int> { 1, 2, 3, 4, 5 };
    Queue<int> loadedUnusedScenes = new Queue<int>();
    int availableServerLayer;

    private void Awake()
    {
        this.gameObject.layer = LayerMask.NameToLayer("SceneChanger");
    }

    public void Start()
    {
        InitiateSceneManager();
    }

    public void InitiateSceneManager()
    {
        if (!NetworkClient.active) { return; }
        if (isServer)
        {
            int defaultScene = SceneUtility.GetBuildIndexByScenePath(NetworkManager.singleton.onlineScene);
            int defaultLayer = 1;
            UseLayer(defaultLayer, defaultScene);
            int defaultSceneIndex = CustomNetworkManager.singleton.GetComponent<CustomNetworkManager>().defaultSceneIndex;
        } // Default scene and default layer

        NetworkClient.RegisterHandler<LayerMessage>(LayerMessageCallback);
        DontDestroyOnLoad(this);
        singleton = this;
    }

    private void LayerMessageCallback(LayerMessage msg)
    {
        ChangeObjectLayer(msg.layerNumber, msg.sceneName);
    }

    /// <summary>
    /// Helper functions for checking which layers are used
    /// </summary>
    /// <returns></returns>

    // Used to get available scene layers that can be used when loading the scene
    public int GetAvailableLayers()
    {
        return layersAvailable.First();
    }

    public void UseLayer(int layer, int sceneBuildIndex)
    {
        layersAvailable.Remove(layer);
        sceneToLayerMapping.Add(sceneBuildIndex, layer);
    }

    public int CheckSceneAlreadyLoaded(int sceneBuildIndex)
    {
        if (sceneToLayerMapping.Keys.ToList().Contains(sceneBuildIndex))
        {
            return sceneToLayerMapping[sceneBuildIndex];
        }
        return -1;
    }

    /// End of functions


    // A function used to recursively change all the gameobject layers upon loading the scene
    private void ChangeObjectLayer(int layerNumber, string sceneName)
    {
        // Change all layers for gameobjects of that scene
        Scene s = SceneManager.GetSceneByName(sceneName);
        GameObject[] gameObjects = s.GetRootGameObjects();
        List<string> layers = new List<string> { "Background", "CombatLayer" };
        foreach (GameObject obj in gameObjects)
        {
            if (LayerMask.LayerToName(obj.layer) == "SceneChanger")
            {
                continue;
            }
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
    public void ServerChangeSceneV2(int sceneBuildIndex, int layerNumber)
    {
        string sceneName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(sceneBuildIndex));
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

        UseLayer(layerNumber, sceneBuildIndex);

        // NetworkServer.SetAllClientsNotReady();

        // Let server prepare for scene change
        NetworkManager.singleton.OnServerChangeScene(sceneName);


        // set server flag to stop processing messages while changing scenes
        // it will be re-enabled in FinishLoadScene.
        NetworkServer.isLoadingScene = true;

        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
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

    [Command(requiresAuthority = false)]
    public void IncrementScenePlayerCount(int previousSceneIndex, int sceneBuildIndex)
    {
        IncrementDictionary(previousSceneIndex, true);

        IncrementDictionary(sceneBuildIndex);

        void IncrementDictionary(int sceneBuildIndex, bool negative = false)
        {
            if (sceneBuildIndex <= 0)
            {
                return;
            }

            if (scenePlayerCount.ContainsKey(sceneBuildIndex))
            {
                scenePlayerCount[sceneBuildIndex] = scenePlayerCount[sceneBuildIndex] + ((negative) ? -1 : 1);
            }
            else
            {
                scenePlayerCount.Add(sceneBuildIndex, 1);
            }
        }
    }

    public List<int> ReturnLoadedScenes()
    {
        return sceneToLayerMapping.Keys.ToList();
    }

    private void ServerLevelLoaded(Scene scene, LoadSceneMode arg1)
    {
        // Used to set all objects of that layer to appropriate layer
        ChangeObjectLayer(availableServerLayer, Path.GetFileNameWithoutExtension(scene.name));
        SceneManager.sceneLoaded -= ServerLevelLoaded;
    }
}
