using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace PlayerAnim
{
    public class PlayerAnimation : NetworkBehaviour
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
            animator.SetTrigger("isAttacking"); // sword slash
            if (!isLocalPlayer)
            {
                Debug.Log("Unlucky");
                return;
            }
            ChangeStateToWarriorLightAttackCommand();
        }

        [Command]
        private void ChangeStateToWarriorLightAttackCommand()
        {
            ChangeStateToWarriorLightAttackClientRPC();
        }

        [ClientRpc(includeOwner = false)]
        private void ChangeStateToWarriorLightAttackClientRPC()
        {
            if (isLocalPlayer) return;
            animator.SetFloat("attackSpeed", 1f);
            animator.SetTrigger("isAttacking"); // sword slash
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void ChangeStateToWarriorHeavyAttack()
        {
            animator.SetFloat("attackSpeed", 0.3f);
            animator.SetTrigger("isAttacking"); // sword slash
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void ChangeStateToRangedAttack()
        {
            // fire an arrow
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void ChangeStateRunning(string direction="")
        {
            animator.SetBool("isRunningHorizontal", (direction == "Horizontal") ? true : false);
            animator.SetBool("isRunningUp", (direction == "Up") ? true : false);
            animator.SetBool("isRunningDown", (direction == "Down") ? true : false);

            if (!isLocalPlayer) return;
            ChangeStateRunningCommand(direction);
        }

        [Command]
        private void ChangeStateRunningCommand(string direction)
        {
            ChangeStateRunningClientRPC(direction);
        }

        [ClientRpc(includeOwner = false)]
        private void ChangeStateRunningClientRPC(string direction)
        {
            if (isLocalPlayer) return;
            animator.SetBool("isRunningHorizontal", (direction == "Horizontal") ? true : false);
            animator.SetBool("isRunningUp", (direction == "Up") ? true : false);
            animator.SetBool("isRunningDown", (direction == "Down") ? true : false);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool CheckHorizontalAnimatorState()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("horizontal_running")
                || animator.GetCurrentAnimatorStateInfo(0).IsName("horizontal_idle")
                || animator.GetCurrentAnimatorStateInfo(0).IsName("horizontal_light_a")) { return true; }
            return false;
        }
    }
}
