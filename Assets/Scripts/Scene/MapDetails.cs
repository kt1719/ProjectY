using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map Detail", menuName = "MapDetail")]
public class MapDetails : ScriptableObject
{
    public Vector2 topLeft;
    public Vector2 bottomRight;
}
