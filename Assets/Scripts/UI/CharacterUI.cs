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

        PlayerController playerController;
        // Start is called before the first frame update
        void Awake()
        {
            healthText = this.transform.Find("HealthText").GetComponent<Text>();
            levelText = this.transform.Find("LevelText").GetComponent<Text>();
            XPText = this.transform.Find("XPText").GetComponent<Text>();
            SkillTreePanel = this.transform.Find("TabMenu").gameObject;
            playerController = this.GetComponentInParent<PlayerController>();
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
                bool skillTreeEnabled = SkillTreePanel.gameObject.activeSelf;
                SkillTreePanel.gameObject.SetActive(!skillTreeEnabled); // Set Skill tree to opposite state
                if (SkillTreePanel.gameObject.activeSelf)
                {
                    playerController.FreezeCharacter();
                }
                else
                {
                    playerController.UnFreezeCharacter();
                }
                playerController.enabled = skillTreeEnabled;
            }
        }
    }
}