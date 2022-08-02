using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerClasses;
using Mirror;
using EnemyClass;

namespace PlayerAtk
{
    public class swordColl : NetworkBehaviour
    {
        private int damage = 0;
        private PlayerClass playerClassScript;
        private Animator animator;
        GameObject swordColliderGameObj;

        private void Awake()
        {
            playerClassScript = GetComponent<PlayerClass>();
            animator = GetComponent<Animator>();
            swordColliderGameObj = transform.GetChild(0).gameObject;
        }
        public void setDamage(int damage)
        {
            this.damage = damage;
        }

        public void HitEventOn()
        {
            // Replace with colliders
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("down_light_a"))
            {
                SetLightAttackCollider(0);
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("horizontal_light_a"))
            {
                SetLightAttackCollider(2);
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("up_light_a"))
            {
                SetLightAttackCollider(1);
            }
        }

        private void SetLightAttackCollider(int index)
        {
            GameObject lightAttackObj = swordColliderGameObj.transform.GetChild(0).gameObject;
            GameObject downAttack = lightAttackObj.transform.GetChild(index).gameObject;
            downAttack.SetActive(true);
            lightAttackObj.SetActive(true);
        }

        public void HitEventOff() // Will have to rework. Just get a reference to the gameobject that was turned on setactive to false on that which will change this from O(n^3) to O(1) both with O(1) memory
        {
            for(int i = 0; i < swordColliderGameObj.transform.childCount; i++)
            {
                GameObject child = swordColliderGameObj.transform.GetChild(i).gameObject;
                if (child.activeSelf)
                {
                    for (int j = 0; j < child.transform.childCount; j++)
                    {
                        GameObject direction = child.transform.GetChild(j).gameObject;
                        direction.SetActive(false);
                    }
                }
                child.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision) // The only triggers that will happen with this player are the sword colliders
        {
            if(!isLocalPlayer)
            {
                return;
            }

            if (collision.tag == "Enemy" && isLocalPlayer)
            {
                //GetComponentInChildren<ScreenShakeController>().StartShake(0.1f, 0.08f);
                EnemyController enemyScript = collision.GetComponent<EnemyController>();
                bool killedEnemy = enemyScript.TakeDamage(damage, this.transform);
                if (killedEnemy) playerClassScript.gainXP(enemyScript.giveXP());
            }
            // if (collision.tag == "Player" && 1v1area)
        }
    }
}
