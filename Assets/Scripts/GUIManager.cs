using System.Collections.Generic;
using UnityEngine;

namespace Maca
{
    public class GUIManager : Singleton<GUIManager>
    {
        [SerializeField]
        public bool isPseudo;
        public int sizeX, sizeY;

        public GameObject introScreen;
        public GameObject mainSelectionsScreen;
        public GameObject gameModeSettingsScreen;
        public GameObject gameIsOnScreen;

        public GameObject letterBox;
        public GameObject numberBox;
        public GameObject blackBox;
        public GameObject emptyBox;

        public GameObject gamePanel;
        public GameObject gameBoard;
        public GameObject gameModes;

        public Color selectedLed;
        public Color selectedLedShadow;
        public Color notSelectedLed;
        public Color notSelectedLedShadow;

        public Transform reference;

        public List<GameObject> boxes = new List<GameObject>();

        private void Awake()
        {
            instance = this;
        }

        public void goMainSelectionScreen()
        {
            introScreen.SetActive(false);
            mainSelectionsScreen.SetActive(true);
            gameModeSettingsScreen.SetActive(false);
            gameIsOnScreen.SetActive(false);
        }

        public void goGameModeSettingScreen()
        {
            introScreen.SetActive(false);
            mainSelectionsScreen.SetActive(false);
            gameModeSettingsScreen.SetActive(true);
            gameIsOnScreen.SetActive(false);

            if(boxes != null)
            {
                destroyPuzzle();
            }
        }

        public void goGameIsOnScreen()
        {
            introScreen.SetActive(false);
            mainSelectionsScreen.SetActive(false);
            gameModeSettingsScreen.SetActive(false);
            gameIsOnScreen.SetActive(true);

            Motor.Instance.createPuzzle();
            GameExecutive.Instance.createGrid();
        }

        private void destroyPuzzle()
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                Destroy(boxes[i]);
            }

            boxes.Clear();
        }
    }
}