using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnim;
using System;
using PlayerCore;
using PlayerAtt;

namespace PlayerClasses
{
    /* Class to define all the abilities for a ranger */
    public class RangerAbility : PlayerAbility
    {
        public float offset;
        public GameObject projectile;
        private PlayerAnimation animatorScript;
        protected delegate void abilityDelegate(); // Function template for an ability, no parameters, void return
        protected Dictionary<int, abilityDelegate> trueAbilityMapping;
        protected Dictionary<int, int> userMouseMapping;
        protected Dictionary<KeyCode, int> userKeyMapping;


        public void Awake() 
        {   
            offset = -90;
            projectile = GetComponent<PlayerController>().prefabs[0];

            animatorScript = GetComponent<PlayerAnimation>();

            trueAbilityMapping = new Dictionary<int, abilityDelegate>()
            {
                {0, new abilityDelegate(LightAttack)},
                // {1, new abilityDelegate(HeavyAttack)},
            };


            /* The following is customisable by the user (settings) */
            userMouseMapping = new Dictionary<int, int>()
            {
                {0, 0}, // left click maps to light attack
                // {1, 1}, // right click maps to heavy attack
            };

            userKeyMapping = new Dictionary<KeyCode, int>() {}; // no ability mappings yet
        }

        /* Invoke the ability associated with the ability id */
        public void UseAbility(int abilityId) // ability id
        {            
            // check that the ability is unlocked
            if (!playerClass.hasUnlocked(abilityId)) {
                return; 
            }
            
            // call the ability associated with the id
            abilityDelegate ability = trueAbilityMapping[abilityId];
            ability();
        }

        /* Check for user input each frame */
        public override void CheckAbility()
        {
            var keycodes = Enum.GetValues(typeof(KeyCode));
            // check for keyboard press
            foreach (KeyCode key in keycodes)
            {   
                //Debug.Log(key);
                bool keydown = Input.GetKeyDown(key);
                bool contain = userKeyMapping.ContainsKey(key);
                if (contain && keydown) {
                    UseAbility(userKeyMapping[key]);
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
            Instantiate(projectile, this.transform.position, GetRotationToCursor());
        }

        private Quaternion GetRotationToCursor()
        {
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

            return Quaternion.Euler(0f, 0f, rotZ + offset);
        }
    }
}