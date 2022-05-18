using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PlayerClasses
{
    // [RequireComponent(typeof(WarriorAbility))]
    public class Ranger : PlayerClass
    {
        public struct RangerStats
        {
            public float Strength { get; set; }
        }

        protected RangerStats rangerstats;

        public override void Awake() 
        {
            base.Awake();

            rangerstats = new RangerStats { Strength = 5 };
            unlockedAbilities = new HashSet<int>() {0, 1};
        }
        public override void LevelUp()
        {
            //TODO: Implement
            base.LevelUp();
            stats.Attack += 1;
            stats.HP += 30;
            return;
        }
    }
}
