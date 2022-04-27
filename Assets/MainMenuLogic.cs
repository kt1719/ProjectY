using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuLogic : MonoBehaviour
    {
        public GameObject gameSelectMenu;
        public GameObject optionsMenu;
        public string singlePlayerScene;
        public string multiPlayerScene;

        private void Start()
        {
            gameSelectMenu = this.transform.GetChild(1).gameObject;
            optionsMenu = this.transform.GetChild(2).gameObject;
        }
        public void SinglePlayerStart()
        {
            SceneManager.LoadScene(singlePlayerScene);
        }

        public void MultiplayerStart()
        {
            SceneManager.LoadScene(multiPlayerScene);
        }

        public void OptionMenuPressed()
        {
            gameSelectMenu.SetActive(!gameSelectMenu.activeSelf);
            optionsMenu.SetActive(!optionsMenu.activeSelf);
        }
    }
}
