 
 
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace EnemyClass
{
    public class EnemyAnimation : NetworkBehaviour
    {
        Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void ChangeStateToTakeDamage()
        {
            animator.SetTrigger("damaged"); // sword slash
        }

        public void ChangeStateToDie()
        {
            animator.SetBool("die", true); // sword slash
        }

        public void ChangeStateToAttack()
        {
            animator.SetTrigger("attack");
        }
    }
}