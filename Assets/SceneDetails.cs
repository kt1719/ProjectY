using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maps
{
    public class SceneDetails : MonoBehaviour
    {
        public MapDetails mapDetail;

        public bool isInBounds (Vector2 vec)
        {
            Vector2 vec1 = mapDetail.bottomRight - vec;
            Vector2 vec2 = vec - mapDetail.topLeft;
            if (vec1.x >= 0 && vec1.y <= 0 && vec2.x > 0 && vec2.y <= 0) { return true; }
            return false;
        }
    }
}