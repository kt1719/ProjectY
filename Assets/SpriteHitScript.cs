using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyClass
{
    public abstract class SpriteHitScript : MonoBehaviour
    {
        Sprite sprite;
        private void Awake()
        {
            sprite = GetComponent<Sprite>();
        }

        public abstract void changeSpriteOrientation(Transform transform);
    }
}
