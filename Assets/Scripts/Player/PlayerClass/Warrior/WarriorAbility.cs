using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnim;
using System;
using PlayerAtk;

namespace PlayerClasses
{
    /* Class to define all the abilities for a warrior */
    public class WarriorAbility : PlayerAbility
    {
        private PlayerAnimation animatorScript; // have to change this specifically to warrior only!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        private PlayerAttack attackScript;
        public delegate void abilityDelegate(); // Function template for an ability, no parameters, void return
        public Dictionary<int, abilityDelegate> trueAbilityMapping;
        public Dictionary<int, int> userMouseMapping;
        public Dictionary<KeyCode, int> userKeyMapping;

        public void Awake() 
        {   
            animatorScript = GetComponent<PlayerAnimation>();
            attackScript = GetComponent<PlayerAttack>();

            trueAbilityMapping = new Dictionary<int, abilityDelegate>()
            {
                {0, new abilityDelegate(LightAttack)},
                {1, new abilityDelegate(HeavyAttack)},
                {2, new abilityDelegate(Blank)},
                {3, new abilityDelegate(Blank)},
                {4, new abilityDelegate(Blank)},
                {5, new abilityDelegate(Blank)},
                {6, new abilityDelegate(Blank)}
            };

            /* The following is customisable by the user (settings) */
            userMouseMapping = new Dictionary<int, int>()
            {
                {0, 0}, // left click maps to light attack
                {1, 1}, // right click maps to heavy attack
            };

            userKeyMapping = new Dictionary<KeyCode, int>()// no ability mappings yet
            {
                {KeyCode.U, 0},
                {KeyCode.I, 1},
            };
        }

        /* Invoke the ability associated with the ability id */
        public void UseAbility(int abilityId) // ability id
        {            
            // check that the ability is unlocked
            if (!playerClass.hasUnlocked(abilityId))
            {
                return; 
            }
            
            // call the ability associated with the id
            abilityDelegate ability = trueAbilityMapping[abilityId];
            ability();
        }

        /* Check for user input each frame */
        public override void CheckAbility()
        {
            // check for keyboard press
            foreach (KeyValuePair<KeyCode, int> keyValue in userKeyMapping)
            {
                if (Input.GetKeyDown(keyValue.Key)) {
                    UseAbility(keyValue.Value);
                }
            }
            // check for mouse key presses
            for (int i = 0; i < 3; i++)
            {
                if (Input.GetMouseButtonDown(i) && userMouseMapping.ContainsKey(i)) {
                    UseAbility(userMouseMapping[i]);
                }
            }
        }

        /*** 
        ----------- Implementation of all warrior abilities ----------------
        ***/
        public void LightAttack() 
        {
            attackScript.Attack(20, animatorScript.ChangeStateToWarriorLightAttack);
        }

        public void HeavyAttack() 
        {
            attackScript.Attack(100, animatorScript.ChangeStateToWarriorHeavyAttack);
        }

        public void Blank()
        {
            return;
        }
    }
}
