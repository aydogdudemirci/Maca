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
        //public GameObject inGame;

        public void goMainSelectionScreen()
        {
            intro.SetActive(false);
            mainSelections.SetActive(true);
            gameModeSettings.SetActive(false);
            //inGame.SetActive(false);
        }

        public void goGameModeSettingScreen()
        {
            intro.SetActive(false);
            mainSelections.SetActive(false);
            gameModeSettings.SetActive(true);
            //inGame.SetActive(false);
        }
    }
}