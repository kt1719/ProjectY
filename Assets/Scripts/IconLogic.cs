using PlayerClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class IconLogic : MonoBehaviour
    {
        Animator animator;
        List<Animator> animatorList = new List<Animator>();


        public enum Colour {
            Red,
            Blue,
            Purple
        }

        public enum States
        {
            Locked,
            SemiUnlocked,
            Unlocked
        }

        public bool upWire;
        public bool downWire;
        public bool leftWire;
        public bool rightWire;
        public Colour col;

        public States unlockedState = States.Locked;
        PlayerClass playerClassScript;

        private void Awake()
        {
            playerClassScript = transform.root.GetComponent<PlayerClass>();
            animator = GetComponent<Animator>();
            this.transform.Find("WireRight").gameObject.SetActive(rightWire);
            this.transform.Find("WireLeft").gameObject.SetActive(leftWire);
            this.transform.Find("WireUp").gameObject.SetActive(upWire);
            this.transform.Find("WireDown").gameObject.SetActive(downWire);
            animator.SetBool(col.ToString(), true);
            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    animatorList.Add(child.gameObject.GetComponent<Animator>());
                }
            }
        }

        public void UnlockAbility()
        {
            if (playerClassScript.unlockAbility(this))
            {
                unlockedState = States.Unlocked;
                animator.SetBool("Unlocked", true);
                foreach (Animator animator in animatorList)
                {
                    animator.SetBool("Unlocked", true);
                }
            }
            else
            {
                Debug.Log("not allowed");
            }
        }

        public void SemiUnlockNext()
        {
            unlockedState = States.SemiUnlocked;
            animator.SetBool("UnlockedNext", true);
        }

        public void InstantAnimatorUnlockState()
        {
            foreach (Animator animator in animatorList)
            {
                animator.Play("Unlocked");
            }
        }

        // Animator resets state every time it is disabled (when the user presses tab)

        private void OnEnable()
        {
            if (unlockedState == States.Locked) return;

            string stateName = col.ToString() + "Icons" + unlockedState.ToString();
            animator.Play(stateName);

            if (unlockedState == States.Unlocked)
            {
                InstantAnimatorUnlockState();
            }
        }
    }
}