using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerClasses;
using Mirror;

namespace PlayerAtt
{
    public class swordColl : MonoBehaviour
    {
        private int damage = 50;
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
                GameObject lightAttackObj = swordColliderGameObj.transform.GetChild(0).gameObject;
                GameObject downAttack = lightAttackObj.transform.GetChild(0).gameObject;
                Debug.Log(lightAttackObj.name);
                downAttack.SetActive(true);
                lightAttackObj.SetActive(true);
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("horizontal_light_a"))
            {
                Debug.Log("Here2");
            }
        }

        public void HitEventOff()
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!this.GetComponent<NetworkBehaviour>().isLocalPlayer)
            {
                return;
            }

            if (collision.tag == "Enemy")
            {
                bool killedEnemy = collision.GetComponent<Enemy>().TakeDamage(damage);
                if (killedEnemy) playerClassScript.gainXP(50);
            }
            // if (collision.tag == "Player" && 1v1area)
        }
    }
}
