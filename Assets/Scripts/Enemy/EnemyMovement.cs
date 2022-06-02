using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Pathfinding;

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

        /////

        int currentWaypoint = 0;
        Path path;

        Seeker seeker;
        bool reachedEndOfPath = false;

        public Transform target;
        /////
        // Start is called before the first frame update
        void Awake()
        {
            speed = stats.speed;
            movementRadius = stats.movementRadius;
            idleTime = Random.Range(0, stats.idleTime); // So every enemy can move at different times
            targetCoord = AsVector2(transform.position);
            rgdbody = GetComponent<Rigidbody2D>();
            seeker = GetComponent<Seeker>();
        }

        private void Start()
        {
            InvokeRepeating("UpdatePath", 0f, .5f);
        }

        void UpdatePath()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            seeker.StartPath(this.transform.position, player.transform.position, OnPathComplete);
        }

        void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                path = p;
                currentWaypoint = 0;
            }
        }

        public void ChasePlayer()
        {
            if (path == null)
            {
                Debug.Log("Not following path");
                AutoMove();
            }
            else
            {
                if (currentWaypoint >= path.vectorPath.Count)
                {
                    // Attack player
                    reachedEndOfPath = true;
                    return;
                }
                else
                {
                    reachedEndOfPath = false;
                }

                Vector2 dir = AsVector2(path.vectorPath[currentWaypoint] - transform.position);
                rgdbody.velocity = speed * dir.normalized;

                float distance = dir.magnitude;

                if (distance < 0.3)
                {
                    currentWaypoint++;
                }
            }
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
                idleTime = Random.Range(0, stats.idleTime);
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