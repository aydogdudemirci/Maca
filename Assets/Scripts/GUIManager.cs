using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

        public GameObject loadScreen;
        public GameObject introScreen;
        public GameObject gameModeSettingsScreen;
        public GameObject gameIsOnScreen;

        public GameObject letterBox;
        public GameObject numberBox;
        public GameObject blackBox;
        public GameObject emptyBox;

        public GameObject gamePanel;
        public GameObject gameBoard;
        public GameObject question;
        public GameObject images;

        public Color selectedLed;
        public Color selectedLedShadow;
        public Color notSelectedLed;
        public Color notSelectedLedShadow;

        public Color highlightLight;
        public Color highlightDark;
        //public Color highlightRed;

        public Transform reference;

        public GameObject loadSquare;

        private void Awake()
        {
            instance = this;
            StartCoroutine(makeLoadScreen(introScreen));
        }

        public void goIntroScreen(GameObject target)
        {
            makeAllScenesDisable();
            Animate(target);
        }

        public void goGameModeSettingScreen(GameObject target)
        {
            GridCreator.Instance.destroyGrid();
            makeAllScenesDisable();
            Animate(target);
        }

        public void goGameIsOnScreen(GameObject target)
        {
            makeAllScenesDisable();
            Motor.Instance.createPuzzle();
            GridCreator.Instance.createGrid();
            StartCoroutine(makeLoadScreen(target));
        }

        private void makeAllScenesDisable()
        {
            introScreen.SetActive(false);
            gameModeSettingsScreen.SetActive(false);
            gameIsOnScreen.SetActive(false);
            loadScreen.SetActive(false);
        }

        private void Animate(GameObject target)
        {
            target.SetActive(true);
            target.GetComponent<Animator>().SetBool("SlideOut", true);
        }

        IEnumerator makeLoadScreen(GameObject target)
        {
            yield return new WaitForEndOfFrame();
            Animate(loadScreen);

            loadSquare.GetComponent<Image>().fillAmount = 0.1f;

            while (loadSquare.GetComponent<Image>().fillAmount < 1)
            {
                loadSquare.GetComponent<Image>().fillAmount += 0.0075f;

                yield return new WaitForEndOfFrame();
            }

            makeAllScenesDisable();
            Animate(target);
        }
    }
}
