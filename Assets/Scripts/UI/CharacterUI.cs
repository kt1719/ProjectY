using PlayerClasses;
using PlayerCore;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CharacterUI : MonoBehaviour
    {
        GameObject SkillTreePanel;
        GameObject EscMenu;

        PlayerController playerController;

        Text SkillTreePoints;
        PlayerClass playerClassScript;

        // Start is called before the first frame update
        void Awake()
        {
            SkillTreePanel = this.transform.Find("TabMenu").transform.Find("SkillTree").gameObject;
            SkillTreePoints = SkillTreePanel.transform.Find("Texts").transform.Find("SkillTreePoints").GetComponent<Text>();
            EscMenu = this.transform.Find("EscMenu").gameObject;
            playerController = this.GetComponentInParent<PlayerController>();
            playerClassScript = this.GetComponentInParent<PlayerClass>();
        }

        // Update is called once per frame
        void Update()
        {
            OpenSkillTree();
            OpenEscMenu();
            UpdateSkillTreePoints();
        }

        private void UpdateSkillTreePoints()
        {
            SkillTreePoints.text = playerClassScript.readPointsAvailable();
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
                if (EscMenu.gameObject.activeSelf || SkillTreePanel.gameObject.activeSelf)
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