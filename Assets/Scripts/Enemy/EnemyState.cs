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
            Idle,
            Attacking,
            Hit
        }

        public States currentState;

        public virtual void changeStateAttacking()
        {
            currentState = States.Attacking;
        }

        public virtual void changeStateIdle()
        {
            currentState = States.Idle;
        }

        public virtual void changeStateHit()
        {
            currentState = States.Hit;
        }
    }
}
