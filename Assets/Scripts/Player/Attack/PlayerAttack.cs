using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnim;
using System;

namespace PlayerAtk
{
    public class PlayerAttack : MonoBehaviour
    {
        protected swordColl swordColl;
        private void Awake()
        {
            swordColl = GetComponent<swordColl>();
        }

        public virtual void Attack(int damage, Action animatorScriptFunction)
        {
            swordColl.setDamage(damage); // one shot ;)
            animatorScriptFunction();
        }
    }
}

