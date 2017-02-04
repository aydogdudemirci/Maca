using UnityEngine;

namespace Maca
{
    public class XYCouple
    {
        public int x;
        public int y;

        public XYCouple(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class GUIManager : Singleton<GUIManager>
    {
        [SerializeField]
        public bool isPseudo;
        public int X, Y;

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

        public Color highlightLight;
        public Color highlightDark;

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
            GridCreator.Instance.destroyGrid();

            introScreen.SetActive(false);
            mainSelectionsScreen.SetActive(false);
            gameModeSettingsScreen.SetActive(true);
            gameIsOnScreen.SetActive(false);
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