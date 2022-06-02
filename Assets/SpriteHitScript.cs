using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyClass
{
    public abstract class SpriteHitScript : MonoBehaviour
    {
        protected SpriteRenderer sprite;
        private void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();
        }

        public abstract void changeSpriteOrientation(Transform transform);
    }
}
