using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace PlayerClasses
{
    public abstract class PlayerClass : MonoBehaviour
    {
        public class BasicStats
        {
            public int totalHP { get; set; }
            public int currentHP { get; set; }
            public float Attack { get; set; }
            public int Level { get; set; }
            public int XP { get; set; }
            public int LevelUpXP { get; set; } // this is the xp that player has to get to level up. Should increase as the player levels up
            public float Speed { get; set; }
            public int PointsAvailable { get; set; }
        }

        protected BasicStats stats;

        protected HashSet<int> lockedAbilities = new HashSet<int>();
        protected HashSet<int> unlockedAbilities = new HashSet<int>();
        protected HashSet<int> semiUnlockedAbilities = new HashSet<int>();
        protected Dictionary<int, IconLogic> abilityIdToIconInstance;
        protected Dictionary<int, HashSet<int>> dependencyMap;

        // To make sure each class is instantiated uniquely
        public abstract void Awake();

        public int readCurrentHP()
        {
            return stats.currentHP;
        }

        public int readTotalHP()
        {
            return stats.totalHP;
        }

        public float getSpeed()
        {
            return stats.Speed;
        }

        public string readLevel()
        {
            return stats.Level.ToString();
        }

        public string readXP()
        {
            return stats.XP.ToString();
        }

        public string readLevelUpXP()
        {
            return stats.LevelUpXP.ToString();
        }

        public string readPointsAvailable()
        {
            return stats.PointsAvailable.ToString();
        }

        public virtual void LevelUp()
        {
            // TODO: fine tune
            stats.Level += 1;
            stats.PointsAvailable += 1;
        }

        public bool hasUnlocked(int abilityId)
        {
            return unlockedAbilities.Contains(abilityId);
        }

        protected void generateSemiUnlockedAbilities()
        {
            foreach (int abilityId in lockedAbilities)
            {
                HashSet<int> dependencies = dependencyMap[abilityId];
                if (dependencies.IsSubsetOf(unlockedAbilities))
                {
                    semiUnlockedAbilities.Add(abilityId);
                }
            }
        }

        protected void generateLockedAbilities()
        {
            for (int i = 0; i < dependencyMap.Count; i++)
            {
                if (!unlockedAbilities.Contains(i))
                {
                    lockedAbilities.Add(i);
                }
            }
        }

        protected void updateSemiUnlockedAbilities()
        {
            generateSemiUnlockedAbilities();
            foreach (int abilityId in semiUnlockedAbilities)
            {
                abilityIdToIconInstance[abilityId].UnlockedNext();
            }
        }

        public bool unlockAbility(IconLogic icon)
        {
            // Helper Functions 
            void UnlockPassiveAbility(int abilityId)
            {
                // if abilityId inside activeAbilities then return

                // Otherwise increase stats
            }

            void UpdateSkillTreeVariables(int abilityId)
            {
                stats.PointsAvailable -= 1;
                unlockedAbilities.Add(abilityId);
                lockedAbilities.Remove(abilityId);
                semiUnlockedAbilities.Remove(abilityId);
            }

            bool CheckAbilityUnlockable(int abilityId)
            {
                if (stats.PointsAvailable <= 0)
                {
                    // Not enough skill points
                    return false;
                }
                if (unlockedAbilities.Contains(abilityId))
                {
                    // Already unlocked
                    return false;
                }
                if (!semiUnlockedAbilities.Contains(abilityId))
                {
                    // Check the semi unlocked array to see if the abilityId is populated
                    return false;
                }
                return true;
            }

            int FindAbilityId(IconLogic icon)
            {
                foreach (KeyValuePair<int, IconLogic> hashMapValue in abilityIdToIconInstance)
                {
                    if (hashMapValue.Value == icon)
                    {
                        return hashMapValue.Key;
                    }

                }
                return -1; // Should never be the case
            }

            ////////////

            int abilityId = FindAbilityId(icon);

            if (!CheckAbilityUnlockable(abilityId)) return false;

            // UnlockPassiveAbility(abilityId); // Need to implement

            UpdateSkillTreeVariables(abilityId);

            updateSemiUnlockedAbilities();
            return true;
        }

        public void damageHP(int dmg)
        {
            Debug.Log("Damaged hp");
            stats.currentHP = ((stats.currentHP - dmg) < 0) ? 0 : stats.currentHP - dmg;
            Debug.Log("New hp now " + stats.currentHP);
        }

        public void healHP(int heal)
        {
            stats.currentHP = ((stats.currentHP - heal) < stats.totalHP) ? stats.totalHP : stats.currentHP + heal;
        }

        public void gainXP(int xp)
        {
            int newXP = stats.XP + xp;
            bool hasLeveled = (newXP >= stats.LevelUpXP) ? true : false;
            while (hasLeveled)
            {
                xp -= (stats.LevelUpXP - stats.XP);
                LevelUp();
                stats.XP = 0;
                if (xp < stats.LevelUpXP)
                {
                    hasLeveled = false;
                }
            }
            stats.XP += xp;
        }
    }
}