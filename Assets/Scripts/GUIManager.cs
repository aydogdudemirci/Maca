using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Maca
{
    public class GUIManager : Singleton<GUIManager>
    {
        [SerializeField]
        public bool isPseudo;
        public int X, Y;

        public GameObject loadScreen;
        public GameObject introScreen;
        public GameObject gameModeSettingsScreen;
        public GameObject gameIsOnScreen;
        public GameObject loadSquare;

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

        public Transform reference;

        public Text percentage;
        public GameObject ibre;

        public RuntimeAnimatorController disappear;
        public RuntimeAnimatorController topDown;

        private void Awake()
        {
            instance = this;
            StartCoroutine(makeLoadScreen(introScreen));
        }

        public void goIntroScreen()
        {
            StartCoroutine(makeTransitionToIntroScreen());
        }

        public void goGameModeScreen(GameObject whereFrom)
        {
            StartCoroutine(makeTransitionToGameModeScreen(whereFrom));
        }

        public void goGameScreen()
        {
            StartCoroutine(makeTransitionToGameScreen());
        }

        private void makeAllScenesDisable()
        {
            introScreen.SetActive(false);
            gameModeSettingsScreen.SetActive(false);
            gameIsOnScreen.SetActive(false);
            loadScreen.SetActive(false);
        }

        private void animateTopdown(GameObject target)
        {
            target.GetComponent<Animator>().runtimeAnimatorController = topDown;
            target.SetActive(true);
            target.GetComponent<Animator>().SetBool("SlideOut", true);
        }

        private void animateDisappear(GameObject target)
        {
            target.GetComponent<Animator>().runtimeAnimatorController = disappear;
            target.SetActive(true);
            target.GetComponent<Animator>().SetBool("SlideOut", true);
        }

        IEnumerator makeLoadScreen(GameObject target)
        {
            yield return new WaitForEndOfFrame();
            float amount = 0.1f;

            animateTopdown(loadScreen);

            loadSquare.GetComponent<Image>().fillAmount = amount;

            while (amount < 1)
            {
                amount += 0.0075f;
                loadSquare.GetComponent<Image>().fillAmount = amount;

                if((int)(amount * 100) % 10 == 0)
                {
                    percentage.text = ((int)(amount * 100)).ToString();
                }
                
                yield return new WaitForEndOfFrame();
            }

            animateDisappear(loadScreen);
            yield return new WaitForSeconds(0.3f);
            makeAllScenesDisable();
            animateTopdown(target);
            loadScreen.GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        private IEnumerator makeTransitionToIntroScreen()
        {
            animateDisappear(gameModeSettingsScreen);
            yield return new WaitForSeconds(0.3f);
            makeAllScenesDisable();
            animateTopdown(introScreen);
            gameModeSettingsScreen.GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        private IEnumerator makeTransitionToGameModeScreen(GameObject whereFrom)
        {
            animateDisappear(whereFrom);
            yield return new WaitForSeconds(0.3f);
            GridCreator.Instance.destroyGrid();
            makeAllScenesDisable();
            animateTopdown(gameModeSettingsScreen);
            whereFrom.GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        private IEnumerator makeTransitionToGameScreen()
        {
            animateDisappear(gameModeSettingsScreen);
            yield return new WaitForSeconds(0.3f);
            makeAllScenesDisable();
            StartCoroutine(makeLoadScreen(gameIsOnScreen));
            Motor.Instance.createPuzzle();
            GridCreator.Instance.createGrid();
            gameModeSettingsScreen.GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        private void Update()
        {
            if (introScreen.activeSelf)
            {
                ibre.transform.Rotate(Vector3.back);
            }
        }
    }
}
