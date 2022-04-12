using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerClasses;

namespace PlayerAtt
{
    public class swordColl : MonoBehaviour
    {
        private int damage;

        public void Start() 
        {
            this.damage = 50;
        }

        public void setDamage(int damage)
        {
            this.damage = damage;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // warriorScript = transform.parent.GetComponent<Warrior>();

            collision.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
