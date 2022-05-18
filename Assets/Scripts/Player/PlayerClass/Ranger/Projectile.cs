
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace PlayerClasses {
    public class Projectile : MonoBehaviour {
        public float speed;
        public float lifeTime;

        public int damage;
        private void Start()
        {
            Invoke("DestroyProjectile", lifeTime);
        }
        private void Update()
        {
            GetComponent<Rigidbody2D>().velocity = transform.up * speed;
        }

        void DestroyProjectile() {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!this.GetComponentInParent<NetworkBehaviour>().isLocalPlayer)
            { //prevent multiple damage
                return;
            }

            if (collision.tag == "Enemy")
            {
                bool killedEnemy = collision.GetComponent<Enemy>().TakeDamage(damage);
                // TODO: if (killedEnemy) playerClassScript.gainXP(50);
                DestroyProjectile();
            }

            // if (collision.tag == "Player" && 1v1area)
        }
    }

}