using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class IconLogic : MonoBehaviour
    {
        Animator animator;

        public bool upWire;
        public bool downWire;
        public bool leftWire;
        public bool rightWire;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            this.transform.Find("WireRight").gameObject.SetActive(rightWire);
            this.transform.Find("WireLeft").gameObject.SetActive(leftWire);
            this.transform.Find("WireUp").gameObject.SetActive(upWire);
            this.transform.Find("WireDown").gameObject.SetActive(downWire);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}