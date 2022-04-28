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
        public GameObject multiplayerMenu;
        public string singlePlayerScene;
        public string multiPlayerScene;

        private void Start()
        {
            gameSelectMenu = this.transform.GetChild(1).gameObject;
            optionsMenu = this.transform.GetChild(2).gameObject;
            multiplayerMenu = this.transform.GetChild(3).gameObject;
        }
        public void SinglePlayerPressed()
        {
            SceneManager.LoadScene(singlePlayerScene);
        }

        public void MultiplayerPressed()
        {
            gameSelectMenu.SetActive(!gameSelectMenu.activeSelf);
            multiplayerMenu.SetActive(!multiplayerMenu.activeSelf);
        }

        public void OptionMenuPressed()
        {
            gameSelectMenu.SetActive(!gameSelectMenu.activeSelf);
            optionsMenu.SetActive(!optionsMenu.activeSelf);
        }
    }
}
