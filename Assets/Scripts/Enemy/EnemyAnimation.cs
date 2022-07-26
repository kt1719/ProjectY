 
 
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace EnemyClass
{
    public class EnemyAnimation : NetworkBehaviour
    {
        protected Animator animator;
        protected EnemyAttackScript attackScript;
        protected EnemyState stateScript;

        void Awake()
        {
            animator = GetComponent<Animator>();
            attackScript = GetComponent<EnemyAttackScript>();
            stateScript = GetComponent<EnemyState>();
        }

        public virtual void ChangeAnimationTakeDamage()
        {
            animator.SetTrigger("damaged"); // sword slash
        }

        public virtual void ChangeAnimationToDie()
        {
            animator.SetBool("die", true); // sword slash
        }

        public virtual void ChangeAnimationToAttack()
        {
            animator.SetTrigger("attack");
        }
    }
}