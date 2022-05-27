using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace PlayerCore
{
    public class GameEvent : NetworkBehaviour
    {
        public delegate void OnEnemyHitTrigger(int damage);
        public static OnEnemyHitTrigger onEnemyHitTrigger;
    }
}
