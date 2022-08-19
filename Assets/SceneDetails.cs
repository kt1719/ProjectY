using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDetails : MonoBehaviour
{
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
