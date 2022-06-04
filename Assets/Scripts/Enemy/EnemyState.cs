using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace EnemyClass
{
    public class EnemyState : NetworkBehaviour
    {
        public enum States
        {
            Moving,
            Attacking,
            Hit
        }

        public States currentState;

        public virtual void changeStateAttacking()
        {
            currentState = States.Attacking;
        }

        public virtual void changeStateMoving()
        {
            currentState = States.Moving;
        }

        public virtual void chanceStateHit()
        {
            currentState = States.Hit;
        }
    }
}
