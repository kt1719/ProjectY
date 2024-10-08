using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PlayerClasses
{
    // [RequireComponent(typeof(NinjaAbility))]
    public class Ninja : PlayerClass
    {
        public struct NinjaStats
        {
            public float Strength { get; set; }
        }

        protected NinjaStats ninjaStats;

        public override void Awake() 
        {
            ninjaStats = new NinjaStats { Strength = 5 };
            unlockedAbilities = new HashSet<int>() {0, 1, 2};
        }
        public override void LevelUp()
        {
            //TODO: Implement
            base.LevelUp();
            stats.Attack += 1;
            stats.currentHP += 30;
            stats.totalHP += 30;
            return;
        }
    }
}
