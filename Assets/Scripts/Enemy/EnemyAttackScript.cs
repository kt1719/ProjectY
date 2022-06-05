using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerClasses;

namespace EnemyClass
{
    public class EnemyAttackScript : MonoBehaviour
    {
        protected int damage;
        protected float attackRadius;
        protected float centerYOffset;

        protected Animator animator;
        public EnemyScriptableObj stats;
        private void Awake()
        {
            damage = stats.damage;
            attackRadius = stats.attackRadius;
            centerYOffset = stats.centerYOffset;
            animator = GetComponent<Animator>();
        }
        public virtual void Attack() { }

        public virtual Collider2D PlayerAttackable()
        {
            LayerMask mask = LayerMask.GetMask("Player");
            Collider2D player = Physics2D.OverlapCircle(this.transform.position + new Vector3(0, centerYOffset, 0), attackRadius, mask.value);
            return player;
        }
    }
}
