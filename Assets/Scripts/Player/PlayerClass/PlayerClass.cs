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
            public int XP { get; set; }
            public int LevelUpXP { get; set; } // this is the xp that player has to get to level up. Should increase as the player levels up
        }

        protected BasicStats stats;

        protected HashSet<int> unlockedAbilities;
        public virtual void Awake()
        {
            stats = new BasicStats { HP = 100, Attack = 5, Level = 1, XP = 0, LevelUpXP = 100 };
        }

        public string readHP()
        {
            return stats.HP.ToString();
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

        public virtual void LevelUp()
        {
            // TODO: fine tune
            stats.Level += 1;
            stats.LevelUpXP += (int)(stats.LevelUpXP * 0.3); // need to tune
        }

        public bool hasUnlocked(int abilityId)
        {
            return unlockedAbilities.Contains(abilityId);
        }

        public void damageHP(int dmg)
        {
            stats.HP = ((stats.HP - dmg) < 0) ? 0 : stats.HP - dmg;
        }

        public void healHP(int heal)
        {
            stats.HP = ((stats.HP - heal) < 100) ? 100 : stats.HP + heal;
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