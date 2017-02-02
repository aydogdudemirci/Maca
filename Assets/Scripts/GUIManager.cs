using System.Collections.Generic;
using UnityEngine;

namespace Maca
{
    public class GUIManager : Singleton<GUIManager>
    {
        [SerializeField]
        public bool isPseudo;
        public int sizeX, sizeY;

        public GameObject Player;
        private GameObject myPlayer;

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

        private void Start()
        {
            myPlayer = null;
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

            destroyPuzzle();
        }

        public void goGameIsOnScreen()
        {
            introScreen.SetActive(false);
            mainSelectionsScreen.SetActive(false);
            gameModeSettingsScreen.SetActive(false);
            gameIsOnScreen.SetActive(true);

            createPlayer();
        }

        private void createPlayer()
        {
            myPlayer = Instantiate(Player, Vector2.zero, Quaternion.identity) as GameObject;
            myPlayer.transform.SetParent(gameObject.transform.parent);
        }

        private void destroyPuzzle()
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                Destroy(boxes[i]);
            }

            boxes.Clear();
            Destroy(myPlayer);
        }
    }
}