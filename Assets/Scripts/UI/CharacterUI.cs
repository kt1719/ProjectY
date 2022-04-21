using PlayerClasses;
using PlayerCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CharacterUI : MonoBehaviour
    {
        public PlayerClass playerClass;

        public Text healthText;
        public Text levelText;
        public Text XPText;

        // Start is called before the first frame update
        void Start()
        {
            healthText = this.transform.Find("HealthText").GetComponent<Text>();
            levelText = this.transform.Find("LevelText").GetComponent<Text>();
            XPText = this.transform.Find("XPText").GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            healthText.text = "Health: " + playerClass.readHP();
            levelText.text = "Level: " + playerClass.readLevel();
            XPText.text = "XP: " + playerClass.readXP() + "/" + playerClass.readLevelUpXP();
        }
    }
}