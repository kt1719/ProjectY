using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerUI
{
    public class PlayerAnimation : MonoBehaviour
    {
        // Start is called before the first frame update
        Animator animator;
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void ChangeStateToAttack()
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack") || !animator.GetCurrentAnimatorStateInfo(0).IsName("attack1"))
            {
                animator.SetTrigger("isAttacking");
            }
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
