using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace EnemyClass
{
    public class EnemyMovement : NetworkBehaviour
    {
        Rigidbody2D rgdbody;

        public float speed;
        public float movementRadius;
        public float idleTime;

        public EnemyScriptableObj stats;
        public Vector2 targetCoord;
        // Start is called before the first frame update
        void Awake()
        {
            speed = stats.speed;
            movementRadius = stats.movementRadius;
            idleTime = Random.Range(0, stats.idleTime); // So every enemy can move at different times
            targetCoord = AsVector2(transform.position);
            rgdbody = GetComponent<Rigidbody2D>();
        }

        public void AutoMove()
        {
            Vector2 distance = targetCoord - AsVector2(transform.position);
            if (distance.magnitude > 0.1)
            {
                rgdbody.velocity = distance.normalized * speed;
            }
            else if (idleTime <= 0)
            {
                idleTime = stats.idleTime;
                targetCoord = GenerateRandomCoord() + targetCoord;
            }
            else
            {
                StopMovement();
                idleTime -= Time.deltaTime;
            }
        }

        Vector2 GenerateRandomCoord()
        {
            return Random.insideUnitCircle * movementRadius;
        }

        void StopMovement()
        {
            rgdbody.velocity = Vector2.zero;
        }

        Vector2 AsVector2(Vector3 vec)
        {
            return new Vector2(vec.x, vec.y);
        }
    }
}