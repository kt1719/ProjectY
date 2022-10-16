using PlayerClasses;
using PlayerCore;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CharacterUI : MonoBehaviour
    {
        public static CharacterUI instance;
        GameObject SkillTreePanel;
        GameObject EscMenu;

        PlayerController playerController;

        Text SkillTreePoints;
        PlayerClass playerClassScript;
        SceneTransition sceneTransitionAnimator;

        private int sceneTransitionReady = 0;
        // Start is called before the first frame update
        void Awake()
        {
            SkillTreePanel = this.transform.Find("TabMenu").transform.Find("SkillTree").gameObject;
            SkillTreePoints = SkillTreePanel.transform.Find("Texts").transform.Find("SkillTreePoints").GetComponent<Text>();
            EscMenu = this.transform.Find("EscMenu").gameObject;
            sceneTransitionAnimator = this.transform.Find("SceneTransition").GetComponent<SceneTransition>();
            playerController = this.GetComponentInParent<PlayerController>();
            playerClassScript = this.GetComponentInParent<PlayerClass>();
        }

        private void Start()
        {
            instance = this;
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

        public void StartTransitionAnimation()
        {
            sceneTransitionAnimator.StartTransitionAnimation();
        }

        public void EndTransitionAnimation()
        {
            sceneTransitionReady = 0;
            sceneTransitionAnimator.EndTransitionAnimation();
        }

        public void SetReadyForTransition()
        {
            if (sceneTransitionReady >= 2)
            {
                TransitionScene();
            }
            else
            {
                sceneTransitionReady += 1;
            }
        }

        private void TransitionScene()
        {
            sceneTransitionReady = 0;
            playerController.UpdateCameraLayer();
            playerController.UpdatePlayerScenePosition();
            playerController.ChangePlayerLayerLocal();
            playerController.UpdateCameraBounds();
            sceneTransitionAnimator.EndTransitionAnimation();
            playerController.UnFreezeCharacter();
        }
    }
}