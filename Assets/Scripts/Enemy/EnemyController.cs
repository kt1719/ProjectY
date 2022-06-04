using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using PlayerCore;

namespace EnemyClass
{
    public class EnemyController : NetworkBehaviour
    {
        private int health;
        private EnemyAnimation animatorScript;
        public EnemyScriptableObj stats;
        private EnemyMovement movementScript;
        private SpriteHitScript spriteHitScript;
        private EnemyState enemyStateScript;
        private int xpGiven;

        float attackRadius;
        float outerRadius;

        public float centerYOffset;
        // Start is called before the first frame update
        void Awake()
        {
            health = stats.health;
            xpGiven = stats.xpGiven;
            movementScript = GetComponent<EnemyMovement>();
            animatorScript = GetComponent<EnemyAnimation>();
            spriteHitScript = GetComponent<SpriteHitScript>();
            enemyStateScript = GetComponent<EnemyState>();

            attackRadius = stats.aggroRadius;
            outerRadius = stats.outerRadius;
            centerYOffset = stats.centerYOffset;
        }

        private void FixedUpdate()
        {
            if (!isServer)
            {
                return;
            }
            movementScript.Agression(outerRadius, attackRadius);
            movementScript.AutoMove();
            movementScript.CalculateNearestPath();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position + new Vector3(0, centerYOffset, 0), attackRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position + new Vector3(0, centerYOffset, 0), outerRadius);
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(this.transform.position + new Vector3(0, centerYOffset, 0), stats.attackRadius);
        }

        [Client]
        public bool TakeDamage(int damage, Transform transform)
        {
            if (health <= 0) { return false; } // This is to fix the bug of the player killing the enemy multiple times after it's dead and has not despawned
            health -= damage;
            TakeDamageCommand(health, transform);
            if (health <= 0)
            {
                animatorScript.ChangeStateToDie();
                return true;
            }
            return false;
        }

        [Command(requiresAuthority = false)]
        public void TakeDamageCommand(int hp, Transform transform)
        {
            TakeDamageRPC(hp, transform);
        }

        [ClientRpc(includeOwner = true)]
        public void TakeDamageRPC(int hp, Transform transform)
        {
            if (isLocalPlayer) return;
            this.health = hp;
            if (health <= 0)
            {
                animatorScript.ChangeStateToDie();
            }
            spriteHitScript.changeSpriteOrientation(transform);
            animatorScript.ChangeStateToTakeDamage();
        }

        // DO NOT CALL, this should be called from the animation timeline and not programmatically
        public void DestroyEnemy()
        {
            Destroy(gameObject);
        }

        public int giveXP()
        {
            return xpGiven;
        }
    }
}
