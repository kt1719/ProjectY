using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerClasses
{
    [RequireComponent(typeof(WarriorAbility))]
    public class Warrior : PlayerClass
    {
        public struct WarriorStats
        {
            public float Strength { get; set; }
        }

        protected WarriorStats warriorstats;

        public override void InstantiateClass()
        {
            stats = new BasicStats { HP = 100, Attack = 5, Level = 1 };
            warriorstats = new WarriorStats { Strength = 5 };
        }

        public void UseAbility(int num)
        {
            //to be implemented by using WarriorAbility script
            return;
        }
    }
}
