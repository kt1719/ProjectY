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
        public PlayerClass playerClassScript;

        private void Awake()
        {
            playerClassScript = GetComponentInParent<PlayerClass>();
        }
        public void setDamage(int damage)
        {
            this.damage = damage;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!this.GetComponentInParent<NetworkBehaviour>().isLocalPlayer)
            {
                return;
            }

            if (collision.tag == "Enemy")
            {
                Debug.Log("Local player took damage");
                bool killedEnemy = collision.GetComponent<Enemy>().TakeDamage(damage);
                if (killedEnemy) playerClassScript.gainXP(50);
            }
            // if (collision.tag == "Player" && 1v1area)
        }
    }
}
