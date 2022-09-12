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

        CanvasScaler canvasScaler;

        // Start is called before the first frame update
        void Awake()
        {
            healthText = this.transform.Find("HealthText").GetComponent<Text>();
            levelText = this.transform.Find("LevelText").GetComponent<Text>();
            XPText = this.transform.Find("XPText").GetComponent<Text>();
            SkillTreePanel = this.transform.Find("TabMenu").gameObject;
            playerController = this.GetComponentInParent<PlayerController>();
            canvasScaler = this.GetComponent<CanvasScaler>();
        }

        /// <summary>
        /// Subscriber Events for the Character UI Script
        /// </summary>
        private void OnEnable()
        {
            EventManager.UpdateResolutionEvent += ChangeRes;
        }

        private void OnDisable()
        {
            EventManager.UpdateResolutionEvent -= ChangeRes;
        }

        void ChangeRes(int x, int y)
        {
            canvasScaler.referenceResolution = new Vector2(x, y);
        }

        // Update is called once per frame
        void Update()
        {
            UpdateUI();
            OpenSkillTree();
        }

        // Should also be a subscriber event
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