using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Maca
{
    public class GUIManager : Singleton<GUIManager>
    {
        public GameObject intro;                     
        public GameObject mainSelections;                       
        public GameObject gameModeSettings;
        public GameObject gameIsOn;

        public GameObject gamePanel;
        GameObject gameStuff;

        public static bool isPseudo;

        private void Awake()
        {
            isPseudo = true;
        }

        public void goMainSelectionScreen()
        {
            intro.SetActive(false);
            mainSelections.SetActive(true);
            gameModeSettings.SetActive(false);
            gameIsOn.SetActive(false);
        }

        public void goGameModeSettingScreen()
        {
            intro.SetActive(false);
            mainSelections.SetActive(false);
            gameModeSettings.SetActive(true);
            gameIsOn.SetActive(false);

            Destroy(gameStuff);
        }

        public void goGameIsOnScreen()
        {
            intro.SetActive(false);
            mainSelections.SetActive(false);
            gameModeSettings.SetActive(false);
            gameIsOn.SetActive(true);

            gameStuff = Instantiate(gamePanel, Vector2.zero, Quaternion.identity) as GameObject;
            gameStuff.transform.SetParent(gameIsOn.transform);
            gameStuff.transform.localScale = new Vector3(1.0f, 1.0f);
            gameStuff.transform.localPosition = new Vector3(-535.0f, 310.0f);
        }
    }
}