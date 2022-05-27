 
 
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace EnemyUI
{
    public class EnemyAnimation : NetworkBehaviour
    {
        Animator animator;

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void ChangeStateToTakeDamage()
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ninja_damaged"))
            {
                animator.SetTrigger("damaged"); // sword slash
            }

        }

        public void ChangeStateToDie()
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ninja_die_sliced"))
            {
                animator.SetTrigger("die"); // sword slash
            }
        }
    }
}