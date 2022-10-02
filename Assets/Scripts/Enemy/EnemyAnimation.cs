 
 
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
        delegate void AnimationDelegate();
        AnimationDelegate animationFunction;
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

        [Command(requiresAuthority = false)]
        public void UpdateAnimation(string option)
        {
            UpdateAnimationRPC(option);
        }

        [ClientRpc]
        private void UpdateAnimationRPC(string option)
        {
            if (option == "damaged")
            {
                animationFunction = ChangeAnimationTakeDamage;
            }
            else if (option == "attack")
            {
                animationFunction = ChangeAnimationToAttack;
            }
            else if (option == "die")
            {
                animationFunction = ChangeAnimationToDie;
            }
            animationFunction();
        }
    }
}