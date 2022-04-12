using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerUI
{
    public class PlayerAnimation : MonoBehaviour
    {
        // TODO: separate ability animations into separate class animations
        // WarriorAnimation, RangedAnimation...

        // Start is called before the first frame update
        Animator animator;
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void ChangeStateToWarriorAttack()
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack") || !animator.GetCurrentAnimatorStateInfo(0).IsName("attack1"))
            {
                animator.SetTrigger("isAttacking"); // sword slash
            }
        }

        public void ChangeStateToRangedAttack()
        {
            // fire an arrow
        }

        public void ChangeStateRunning()
        {
            animator.SetBool("isRunning", true);
        }

        public void ChangeStateNotRunning()
        {
            animator.SetBool("isRunning", false);
        }

        public void HitEventOn()
        {
            GetComponentInChildren<PolygonCollider2D>().enabled = true;
        }

        public void HitEventOff()
        {
            GetComponentInChildren<PolygonCollider2D>().enabled = false;
        }
    }
}
