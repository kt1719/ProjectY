using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyUI;
using Mirror;
using PlayerCore;

namespace EnemyClass
{
    public class Enemy : NetworkBehaviour
    {
        [SyncVar]
        public int health;
        private EnemyAnimation animatorScript;
        // Start is called before the first frame update
        void Awake()
        {
            health = 100;
            animatorScript = GetComponent<EnemyAnimation>();
        }

        [Client]
        public bool TakeDamage(int damage)
        {
            if (health <= 0) { return false; } // This is to fix the bug of the player killing the enemy multiple times after it's dead and has not despawned
            health -= damage;
            TakeDamageCommand(health);
            if (health <= 0)
            {
                animatorScript.ChangeStateToDie();
                return true;
            }
            animatorScript.ChangeStateToTakeDamage();
            return false;
        }

        [Command(requiresAuthority = false)] // Currently not working
        public void TakeDamageCommand(int hp)
        {
            this.health = hp;
            if (health <= 0)
            {
                animatorScript.ChangeStateToDie();
            }
            animatorScript.ChangeStateToTakeDamage();
        }

        // DO NOT CALL, this should be called from the animation timeline and not programmatically
        public void DestroyEnemy()
        {
            Destroy(gameObject);
        }
    }
}
