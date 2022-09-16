using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UI;

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

        public WarriorBaseStats warriorStats;

        /* All the Icon script mappings for each ability */

        public IconLogic lightAttack;
        public IconLogic heavyAttack;
        public IconLogic none1;
        public IconLogic none2;
        public IconLogic none3;
        public IconLogic none4;
        public IconLogic none5;
        public IconLogic none6;
        public IconLogic none7;
        public IconLogic none8;

        /* */

        public override void Awake()
        {
            GenerateAbilityMappings();

            // Attempt to load the stats or instantiate new ones from scratch


            // If player is not loading a game already saved
            InstaniatePlayerStats();
            InstantiateAbilities();
        }

        private void Start()
        {
            foreach (int abilityId in semiUnlockedAbilities)
            {
                if (!unlockedAbilities.Contains(abilityId))
                {
                    abilityIdToIconInstance[abilityId].unlockedState = IconLogic.States.SemiUnlocked;
                }
            }
            foreach (int abilityId in unlockedAbilities)
            {
                abilityIdToIconInstance[abilityId].unlockedState = IconLogic.States.Unlocked;
            }
        }

        private void InstantiateAbilities()
        {
            unlockedAbilities = new HashSet<int>() { 0, 3 };
            generateLockedAbilities();
            generateSemiUnlockedAbilities();
        }

        private void InstaniatePlayerStats()
        {
            warriorstats = new WarriorStats { Strength = warriorStats.strength };
            stats = new BasicStats { HP = warriorStats.health, Attack = 5, Level = 1, XP = 0, LevelUpXP = 100, Speed = warriorStats.speed, PointsAvailable = 5 };
        }

        private void GenerateAbilityMappings()
        {
            abilityIdToIconInstance = new Dictionary<int, IconLogic>()
            {
                { 0, lightAttack },
                { 1, heavyAttack },
                { 2, none1 },
                { 3, none2 },
                { 4, none3 },
                { 5, none4 },
                { 6, none5 },
                { 7, none6 },
                { 8, none7 },
                { 9, none8 },
            };

            dependencyMap = new Dictionary<int, HashSet<int>>()
            {
                { 0, new HashSet<int>() {} },
                { 1, new HashSet<int>() {0} },
                { 2, new HashSet<int>() {1} },
                { 3, new HashSet<int>() {} },
                { 4, new HashSet<int>() {3} },
                { 5, new HashSet<int>() {4} },
                { 6, new HashSet<int>() {4} },
                { 7, new HashSet<int>() {} },
                { 8, new HashSet<int>() {7} },
                { 9, new HashSet<int>() {8} }
            };
        }
        public override void LevelUp()
        {
            //TODO: Implement
            base.LevelUp();
            stats.Attack += 1;
            stats.HP += 30;
            stats.LevelUpXP += (int)(stats.LevelUpXP * 0.3); // need to tune
            return;
        }
    }
}
