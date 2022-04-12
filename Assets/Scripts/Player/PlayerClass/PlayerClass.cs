using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerClasses
{
    public abstract class PlayerClass : MonoBehaviour
    {
        public struct BasicStats
        {
            public int HP { get; set; }
            public float Attack { get; set; }
            public int Level { get; set; }
        }

        protected BasicStats stats;

        public PlayerClass() 
        {
            stats = new BasicStats { HP = 100, Attack = 5, Level = 1 };
        }

        public virtual void LevelUp()
        {
            stats.Level += 1;
            stats.Attack += 1;
            stats.HP += 30;
        }

        public abstract void CheckAbility();
    }
}