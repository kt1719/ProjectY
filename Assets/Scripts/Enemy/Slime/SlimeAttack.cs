using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerClasses;

namespace EnemyClass
{
    public class SlimeAttack : EnemyAttackScript
    {
        public void DamagePlayer()
        {
            Collider2D player = PlayerAttackable();
            if (player != null)
            {
                player.GetComponent<PlayerClass>().damageHP(damage);
            }
        }
    }
}
