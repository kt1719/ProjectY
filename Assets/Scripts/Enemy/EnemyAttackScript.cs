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
        public float cooldownTimer = 0;

        public EnemyScriptableObj stats;
        private EnemyState stateScript;
        private EnemyAnimation animationScript;
        private void Awake()
        {
            damage = stats.damage;
            attackRadius = stats.attackRadius;
            centerYOffset = stats.centerYOffset;
            stateScript = GetComponent<EnemyState>();
            animationScript = GetComponent<EnemyAnimation>();
        }
        public virtual void Attack() 
        {
            if (cooldownTimer == 0 && stateScript.currentState == EnemyState.States.Idle)
            {
                stateScript.changeStateAttacking();
                animationScript.UpdateAnimation("attack");
                ActivateAttackCooldown();
            }
        }

        public virtual void CountDownCooldown()
        {
            if (stateScript.currentState == EnemyState.States.Attacking)
            {
                return;
            }
            else if (cooldownTimer <= 0)
            {
                cooldownTimer = 0;
                return;
            }
            else
            {
                cooldownTimer -= Time.deltaTime;
            }
        }

        public virtual void ActivateAttackCooldown()
        {
            cooldownTimer = stats.attackCooldown;
        }

        public virtual Collider2D PlayerAttackable()
        {
            LayerMask mask = LayerMask.GetMask("Player");
            Collider2D player = Physics2D.OverlapCircle(this.transform.position + new Vector3(0, centerYOffset, 0), attackRadius, mask.value);
            return player;
        }
    }
}
