using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomSceneManager : NetworkBehaviour
{
    public static CustomSceneManager singleton;

    public void Start()
    {
        Object.DontDestroyOnLoad(this);
        singleton = this;
    }

    Dictionary<string, int> sceneToLayerMapping;
    HashSet<int> layersAvailable = new HashSet<int> { 1, 2, 3, 4, 5 };

    public int GetAvailableLayers()
    {
        return 3;
    }

    public void MoveToLayer(Transform root, int layer)
    {
        root.gameObject.layer = layer;
        foreach (Transform child in root)
        {
            MoveToLayer(child, layer);
        }
    }
}
