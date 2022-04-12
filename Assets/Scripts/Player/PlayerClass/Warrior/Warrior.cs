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
        protected WarriorAbility warriorAbility;
        protected HashSet<int> unlockedAbilities;

        public Warrior(WarriorAbility warriorAbility) : base()
        {
            this.warriorAbility = warriorAbility;
            warriorstats = new WarriorStats { Strength = 5 };
            
            unlockedAbilities = new HashSet<int>() {0, 1};
        }

        public void UseAbility(int id) // ability id
        {
            /*
            // user mapping galeforce 3  <--- idk how to do this mapping tho, idk how unity works; we'll figure it out, doesnt sound impossible
            3 => useAbility(107);
            */
            
            //to be implemented by using WarriorAbility script
            switch(id) 
            {
                case 0:
                    //somethin
                    warriorAbility.LightAttack();
                    break;
                case 1:
                    warriorAbility.HeavyAttack();
                    break;
                default:
                    //should not be here cus there shld be an ability for each id
                    break;
            }
            return;
        }

        public override void LevelUp()
        {
            //TODO: Implement
            return;
        }

        public override void CheckAbility()
        {
            //bool dash = (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && movementState != States.Dashing;


            /*
                keyPress = getKeyPress()
                abilityMapping (key => ability id)  .getId()

            */

            // BTW to eliminate chance 
            if(Input.GetKeyDown(KeyCode.Alpha1)) {
                warriorAbility.LightAttack();
            }
            if(Input.GetKeyDown(KeyCode.Alpha2)) {
                warriorAbility.HeavyAttack();
            }
        
            
            if (!unlockedAbilities.Contains(id)) {
                //ability not unlocked
                return; 
            }

        }
    }
}
