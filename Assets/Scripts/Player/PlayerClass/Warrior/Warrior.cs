using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PlayerClasses
{
    // [RequireComponent(typeof(WarriorAbility))]
    public class Warrior : PlayerClass
    {
        public struct WarriorStats
        {
            public float Strength { get; set; }
        }

        protected WarriorStats warriorstats;

        public override void Start() 
        {
            base.Start();

            warriorstats = new WarriorStats { Strength = 5 };
            unlockedAbilities = new HashSet<int>() {0, 1};
        }
        public override void LevelUp()
        {
            //TODO: Implement
            base.LevelUp();
            return;
        }
    }
}
