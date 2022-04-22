using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerClasses;

namespace PlayerAtt
{
    public class swordColl : MonoBehaviour
    {
        private int damage = 50;
        public PlayerClass playerClassScript;

        private void Start()
        {
            playerClassScript = GetComponentInParent<PlayerClass>();
        }
        public void setDamage(int damage)
        {
            this.damage = damage;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            bool killedEnemy = collision.GetComponent<Enemy>().TakeDamage(damage);
            if (killedEnemy) playerClassScript.gainXP(50);
        }
    }
}
