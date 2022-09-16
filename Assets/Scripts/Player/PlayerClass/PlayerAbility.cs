using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnim;
using Mirror;

namespace PlayerClasses
{
    /* Class to define all the abilities for a warrior */
    public abstract class PlayerAbility : NetworkBehaviour
    {
        protected PlayerClass playerClass;

        public virtual void initialize(PlayerClass playerClass) {
            this.playerClass = playerClass;
        }
        
        public abstract void CheckAbility();

    }
}
