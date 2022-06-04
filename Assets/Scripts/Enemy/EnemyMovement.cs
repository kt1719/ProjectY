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

        private GameObject target;
        bool reachedEndOfPath = true;

        /////
        // Start is called before the first frame update
        void Awake()
        {
            speed = stats.aggroSpeed;
            movementRadius = stats.movementRadius;
            idleTime = Random.Range(0, stats.idleTime); // So every enemy can move at different times
            targetCoord = AsVector2(transform.position);
            rgdbody = GetComponent<Rigidbody2D>();
            seeker = GetComponent<Seeker>();
        }

        void UpdatePath()
        {
            seeker.StartPath(GetCenterPoint(), target.transform.position, OnPathComplete);
        }

        void UpdateIdlePath(Vector3 point)
        {
            seeker.StartPath(GetCenterPoint(), point, OnPathComplete);
        }

        void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                path = p;
                currentWaypoint = 0;
            }
        }

        public void Agression(float outerRadius, float attackRadius)
        {
            if (target != null || IsInvoking("UpdatePath"))
            {
                float distanceFromPlayer = (AsVector2(target.transform.position) - GetCenterPoint()).magnitude;
                if (distanceFromPlayer > outerRadius)
                {
                    StopAI();
                }
                return;
            }

            bool found = FindNearestPlayer(outerRadius, attackRadius);
            if (found)
            {
                speed = stats.aggroSpeed;
                InvokeRepeating("UpdatePath", 0f, .3f);
            }
        }

        private void StopAI()
        {
            target = null;
            path = null;
            currentWaypoint = 0;
            CancelInvoke("UpdatePath");
            reachedEndOfPath = true;
        }

        private bool FindNearestPlayer(float outerRadius, float attackRadius)
        {
            Collider2D[] colliders;
            colliders = Physics2D.OverlapCircleAll(GetCenterPoint(), outerRadius);
            float distanceToPlayer = float.MaxValue;
            bool foundPlayer = false;

            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.tag == "Player")
                {
                    float dist = (GetCenterPoint() - AsVector2(collider.transform.position)).magnitude;
                    if (dist > attackRadius || dist >= distanceToPlayer)
                    {
                        continue;
                    }
                    distanceToPlayer = dist;
                    target = collider.gameObject;
                    foundPlayer = true;
                }
            }
            return foundPlayer;
        }

        public void AutoMove()
        {
            if (target == null)
            {
                IdleMove();
            }
        }

        public void CalculateNearestPath()
        {
            if (path == null)
            {
                return;
            }
            else if (currentWaypoint >= path.vectorPath.Count)
            {
                // Attack player
                reachedEndOfPath = true;
                return;
            }
            else
            {
                reachedEndOfPath = false;
            }

            Vector2 dir = AsVector2(path.vectorPath[currentWaypoint]) - GetCenterPoint();
            rgdbody.velocity = speed * dir.normalized;

            float distance = dir.magnitude;

            if (distance < 0.15)
            {
                currentWaypoint++;
            }
        }

        public void IdleMove()
        {
            if (!reachedEndOfPath && path != null)
            {
                return;
            }
            if (idleTime <= 0 && reachedEndOfPath)
            {
                idleTime = Random.Range(0, stats.idleTime);
                targetCoord = GenerateRandomCoord() + GetCenterPoint();
                UpdateIdlePath(targetCoord);
                reachedEndOfPath = false;
                speed = Random.Range(stats.walkSpeed, stats.aggroSpeed);
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

        Vector2 GetCenterPoint()
        {
            return AsVector2(this.transform.position + new Vector3(0, stats.centerYOffset, 0));
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