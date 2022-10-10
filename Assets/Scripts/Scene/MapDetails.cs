using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map Detail", menuName = "MapDetail")]
public class MapDetails : ScriptableObject
{
    public Vector2 topLeft;
    public Vector2 bottomRight;
    
    [Serializable]
    public struct scenePositionMapping
    {
        public int sceneId;
        public Vector2 position;
    }

    public List<scenePositionMapping> sceneTransitionMapping;
}
