using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDetails : MonoBehaviour
{
    private void Awake()
    {
        this.name += this.gameObject.scene.name;
    }
    public MapDetails mapDetails;

    public Vector2 GetTopLeftBound()
    {
        return mapDetails.topLeft;
    }

    public Vector2 GetBottomRightBounds()
    {
        return mapDetails.bottomRight;
    }
}
