using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerClasses
{
    public class PlayerClass
    {
        public struct BasicStats
        {
            public int HP { get; set; }
            public float Attack { get; set; }
            public int Level { get; set; }
        }

        protected BasicStats stats;

        public virtual void InstantiateClass()
        {
            stats = new BasicStats { HP = 100, Attack = 5, Level = 1 };
        }

        public void LevelUp()
        {
            stats.Level += 1;
            stats.Attack += 1;
            stats.HP += 30;
        }
    }
}