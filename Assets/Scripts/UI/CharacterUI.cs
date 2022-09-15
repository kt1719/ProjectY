using PlayerClasses;
using PlayerCore;
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
        public Text PointsAvailable;

        GameObject SkillTreePanel;
        GameObject EscMenu;

        PlayerController playerController;

        // Start is called before the first frame update
        void Awake()
        {
            healthText = this.transform.Find("HealthText").GetComponent<Text>();
            levelText = this.transform.Find("LevelText").GetComponent<Text>();
            XPText = this.transform.Find("XPText").GetComponent<Text>();
            SkillTreePanel = this.transform.Find("TabMenu").transform.Find("SkillTree").gameObject;
            PointsAvailable = SkillTreePanel.transform.Find("Texts").Find("SkillTreePoints").GetComponent<Text>();
            EscMenu = this.transform.Find("EscMenu").gameObject;
            playerController = this.GetComponentInParent<PlayerController>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateUI();
            OpenSkillTree();
            OpenEscMenu();
        }

        // Should also be a subscriber event
        private void UpdateUI()
        {
            healthText.text = "Health: " + playerClass.readHP();
            levelText.text = "Level: " + playerClass.readLevel();
            XPText.text = "XP: " + playerClass.readXP() + "/" + playerClass.readLevelUpXP();
            PointsAvailable.text = playerClass.readPointsAvailable();
        }

        private void OpenSkillTree()
        {
            if (EscMenu.gameObject.activeSelf)
            {
                return;
            }
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

        private void OpenEscMenu()
        {
            bool escapeEnabled = Input.GetKeyDown(KeyCode.Escape);
            if (escapeEnabled)
            {
                bool escActive = EscMenu.gameObject.activeSelf;
                EscMenu.gameObject.SetActive(!escActive); // Set Skill tree to opposite state
                if (EscMenu.gameObject.activeSelf)
                {
                    playerController.FreezeCharacter();
                }
                else
                {
                    playerController.UnFreezeCharacter();
                }
                playerController.enabled = escActive;
            }
        }
    }
}