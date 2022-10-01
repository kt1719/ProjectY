using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnim;
using System;
using PlayerClasses;

namespace PlayerAtk
{
    public class PlayerAttack : MonoBehaviour
    {
        protected swordColl swordColl;
        PlayerUI playerUIScript;
        PlayerClass playerClassScript;
        private void Awake()
        {
            swordColl = GetComponent<swordColl>();
            playerUIScript = this.transform.Find("PlayerUI").GetComponent<PlayerUI>();
            playerClassScript = GetComponent<PlayerClass>();
        }

        public virtual void Attack(int damage, Action animatorScriptFunction)
        {
            swordColl.setDamage(damage); // one shot ;)
            animatorScriptFunction();
        }

        public void DamagePlayer(int n)
        {
            // Update UI
            playerUIScript.DamageHPUI(n);
            // Update stats
            playerClassScript.damageHP(n);
        }
    }
}

