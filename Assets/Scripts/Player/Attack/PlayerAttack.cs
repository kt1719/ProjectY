using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerUI;

namespace PlayerAtt
{
    public class PlayerAttack : MonoBehaviour
    {
        // Start is called before the first frame update
        private PlayerAnimation animatorScript;
        void Start()
        {
            animatorScript = GetComponent<PlayerAnimation>();
        }

        // Update is called once per frame
        void Update()
        {
            bool attack = Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.M);

            if (attack)
            {
                Attack();
            }
        }

        private void Attack()
        {
            animatorScript.ChangeStateToAttack();
        }

        
    }
}

