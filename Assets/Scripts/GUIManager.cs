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

            GridCreator.Instance.destroyGrid();
        }

        public void goGameIsOnScreen()
        {
            introScreen.SetActive(false);
            mainSelectionsScreen.SetActive(false);
            gameModeSettingsScreen.SetActive(false);
            gameIsOnScreen.SetActive(true);

            Motor.Instance.createPuzzle();
            GridCreator.Instance.createGrid();
        }
    }
}