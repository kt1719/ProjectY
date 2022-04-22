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

        GameObject SkillTreePanel;
        // Start is called before the first frame update
        void Start()
        {
            healthText = this.transform.Find("HealthText").GetComponent<Text>();
            levelText = this.transform.Find("LevelText").GetComponent<Text>();
            XPText = this.transform.Find("XPText").GetComponent<Text>();
            SkillTreePanel = this.transform.Find("SkillTree").gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            UpdateUI();
            OpenSkillTree();
        }

        private void UpdateUI()
        {
            healthText.text = "Health: " + playerClass.readHP();
            levelText.text = "Level: " + playerClass.readLevel();
            XPText.text = "XP: " + playerClass.readXP() + "/" + playerClass.readLevelUpXP();
        }

        private void OpenSkillTree()
        {
            bool skillTreePressed = Input.GetKeyDown(KeyCode.Tab);
            if (skillTreePressed)
            {
                if (SkillTreePanel.gameObject.activeSelf)
                {
                    SkillTreePanel.gameObject.SetActive(false);
                }
                else
                {
                    SkillTreePanel.gameObject.SetActive(true);
                }
            }
        }
    }
}