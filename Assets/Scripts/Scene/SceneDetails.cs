using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneDetails : MonoBehaviour
{
    public MapDetails mapDetails;
    private void Awake()
    {
        this.name += this.gameObject.scene.buildIndex.ToString();
    }

    public Vector2 GetTopLeftBound()
    {
        return mapDetails.topLeft;
    }

    public Vector2 GetBottomRightBounds()
    {
        return mapDetails.bottomRight;
    }

    public Vector2 FindSceneTransitionPosition(int sceneId)
    {
        Debug.Log(sceneId);
        return mapDetails.sceneTransitionMapping.Where(x => x.sceneId == sceneId).ToArray()[0].position;
    }
}
