using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerAnim
{
    public class PlayerAnimation : MonoBehaviour
    {
        // TODO: separate ability animations into separate class animations
        // WarriorAnimation, RangedAnimation...

        // Start is called before the first frame update
        Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void ChangeStateToWarriorLightAttack()
        {
            animator.SetFloat("attackSpeed", 1f);
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack") && !animator.GetCurrentAnimatorStateInfo(0).IsName("attack1"))
            {
                animator.SetTrigger("isAttacking"); // sword slash
            }
        }

        public void ChangeStateToWarriorHeavyAttack()
        {
            animator.SetFloat("attackSpeed", 0.3f);
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack") && !animator.GetCurrentAnimatorStateInfo(0).IsName("attack1"))
            {
                animator.SetTrigger("isAttacking"); // sword slash
            }

        }

        public void ChangeStateToRangedAttack()
        {
            // fire an arrow
        }

        public void ChangeStateRunning(string direction="")
        {
            animator.SetBool("isRunningHorizontal", (direction == "Horizontal") ? true : false);
            animator.SetBool("isRunningUp", (direction == "Up") ? true : false);
            animator.SetBool("isRunningDown", (direction == "Down") ? true : false);
        }

        public bool CheckHorizontalAnimatorState()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("horizontal_running") || animator.GetCurrentAnimatorStateInfo(0).IsName("horizontal_idle")) { return true; }
            return false;
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
