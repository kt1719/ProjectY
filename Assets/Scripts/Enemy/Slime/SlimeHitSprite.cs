using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyClass
{
    public class SlimeHitSprite : SpriteHitScript
    {
        public override void changeSpriteOrientation(Transform transform)
        {
            Vector2 pos = transform.position - this.transform.position;
            if (pos.x > 0)
            {
                sprite.flipX = true;
            }
            else if (pos.x <= 0)
            {
                sprite.flipX = false;
            }
            else
            {
                Debug.Log("Should never go here");
            }
        }
    }
}
