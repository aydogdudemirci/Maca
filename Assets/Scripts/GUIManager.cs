using System.Collections;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Maca
{
    public class GUIManager : Singleton<GUIManager>
    {
        public GameObject loadScreen;
        public GameObject introScreen;
        public GameObject gameModeSettingsScreen;
        public GameObject gameIsOnScreen;
        public GameObject loadSquare;
        public GameObject loadSquareFiller;
        public GameObject vane;

        public RectTransform panel;
        public RectTransform text;

        public Text percentage;
        public GameObject fullPercent;

        public RuntimeAnimatorController disappear;
        public RuntimeAnimatorController topDown;

        public Color start;
        public Color end;
        public Color emergency;
        public Color relax;

        Thread GO;
        public static Stopwatch totalTime;

        public static bool isContinue;
        public static bool isTimeout;

        private void Awake ()
        {
            instance = this;
        }

        private void Start ()
        {
            isContinue = false;
            isTimeout = false;

            totalTime = new Stopwatch ();
            GO = new Thread ( Generator.Instance.generateCrossword );

            StartCoroutine ( makeLoadScreen ( introScreen ) );
        }

        public void goIntroScreen ()
        {
            StartCoroutine ( makeTransitionToIntroScreen () );
        }

        public void goGameModeScreen ( GameObject whereFrom )
        {
            StartCoroutine ( makeTransitionToGameModeScreen ( whereFrom ) );
        }

        public void goGameScreen ()
        {
            if(!BackgroundGenerator.Instance.onProgress)
            {
                if ( introScreen.activeSelf )
                {
                    isContinue = true;
                }

                StartCoroutine ( makeTransitionToGameScreen () );
            }
        }

        private void makeAllScenesDisable ()
        {
            introScreen.SetActive ( false );
            gameModeSettingsScreen.SetActive ( false );
            gameIsOnScreen.SetActive ( false );
            loadScreen.SetActive ( false );
        }

        public void setQuestionToScreen()
        {
            if( panel.sizeDelta.y.Equals(100f) )
            {
                panel.sizeDelta = new Vector2 ( panel.sizeDelta.x, 300f );
                text.sizeDelta = new Vector2 ( text.sizeDelta.x, 300f );
            }

            else
            {
                panel.sizeDelta = new Vector2 ( panel.sizeDelta.x, 100f );
                text.sizeDelta = new Vector2 ( text.sizeDelta.x, 100f );
            }
        }

        private void animateTopdown ( GameObject target )
        {
            target.GetComponent<Animator> ().runtimeAnimatorController = topDown;
            target.SetActive ( true );
            target.GetComponent<Animator> ().SetBool ( "SlideOut", true );
        }

        private void animateDisappear ( GameObject target )
        {
            target.GetComponent<Animator> ().runtimeAnimatorController = disappear;
            target.SetActive ( true );
            target.GetComponent<Animator> ().SetBool ( "SlideOut", true );
        }

        IEnumerator makeLoadScreen ( GameObject target )
        {
            yield return new WaitForEndOfFrame ();

            float amount;
            int old;

            if (!target.name.Equals("GameIsOn"))
            {
                amount = 0.30f;
                loadSquare.GetComponent<Image> ().fillAmount = amount;
                animateTopdown ( loadScreen );

                while ( amount < 1 )
                {
                    old = ( int ) ( amount * 10 );
                    amount += 0.02f;
                    loadSquare.GetComponent<Image> ().fillAmount = amount;

                    if ( old != (int) amount * 10 )
                    {
                        percentage.text = ( old * 10 + 10 ).ToString ();
                    }

                    yield return new WaitForEndOfFrame ();
                }
            }

            else
            {
                Generator.Instance.savePreferences ();

                if(GO.IsAlive)
                {
                    GO.Abort ();
                }

                Generator.Instance.setDatabaseConnection ();

                GO = new Thread ( Generator.Instance.generateCrossword );
                GO.IsBackground = true;

                totalTime.Start ();
                GO.Start ();

                animateTopdown ( loadScreen );

                fullPercent.SetActive (false);
                loadSquare.GetComponent<Image> ().fillAmount = 1;
                animateTopdown ( loadScreen );

                while ( !Generator.Instance.isComplete )
                {
                    loadSquare.GetComponent<Image> ().color = Color.Lerp ( start, end, Mathf.PingPong ( Time.time, 1 ) );
                    yield return new WaitForSeconds (0.01f);
                }

                yield return new WaitForSeconds (0.5f);

                Generator.Instance.isComplete = false;

                if ( isTimeout )
                {
                    if(GO.IsAlive)
                    {
                        GO.Abort ();
                    }

                    GO = new Thread ( Generator.Instance.generateCrossword );
                    GO.IsBackground = true;
                    GO.Start ();

                    while ( !Generator.Instance.isComplete && !Generator.Instance.notGenerated )
                    {
                        loadSquare.GetComponent<Image> ().color = Color.Lerp ( end, emergency, Mathf.PingPong ( Time.time * 2, 0.5f ) );
                        yield return new WaitForSeconds ( 0.01f );
                    }

                    Generator.Instance.isComplete = false;

                    if ( Patterns.reversingNecessary () && !Generator.Instance.notGenerated)
                    {
                        Generator.Instance.reverseCrossword ();
                    }
                }

                else
                {
                    Generator.Instance.isComplete = false;

                    if ( Patterns.reversingNecessary () && !isContinue )
                    {
                        Generator.Instance.reverseCrossword ();
                    }

                    Stopwatch relaxTime = new Stopwatch();

                    relaxTime.Start ();

                    while ( true )
                    {
                        if ( relaxTime.Elapsed.TotalSeconds > 1.5f )
                        {
                            break;
                        }

                        loadSquare.GetComponent<Image> ().color = Color.Lerp ( start, relax, Mathf.PingPong ( Time.time * 2, 0.5f ) );
                        yield return new WaitForSeconds ( 0.01f );
                    }

                    relaxTime.Stop ();
                }

                isContinue = false;
                isTimeout = false;

                if(!Generator.Instance.notGenerated)
                {
                    Generator.Instance.isComplete = false;

                    GamePlay.Instance.createCrossword ();

                    loadSquareFiller.gameObject.SetActive ( true );
                    loadSquareFiller.GetComponent<Image> ().fillAmount = 0;
                    loadSquareFiller.GetComponent<Image> ().color = loadSquare.GetComponent<Image> ().color;

                    while ( loadSquareFiller.GetComponent<Image> ().fillAmount < 1 )
                    {
                        loadSquareFiller.GetComponent<Image> ().fillAmount += 0.05f;
                        yield return new WaitForEndOfFrame ();
                    }
                }

                totalTime.Stop ();
                totalTime.Reset ();
            }

            if( !Generator.Instance.notGenerated )
            {
                animateDisappear ( loadScreen );
                yield return new WaitForSeconds ( 0.4f );

                loadSquareFiller.gameObject.SetActive ( false );
                makeAllScenesDisable ();
                animateTopdown ( target );
                loadScreen.GetComponent<Transform> ().localScale = new Vector3 ( 1.0f, 1.0f, 1.0f );
            }

            else
            {
                animateDisappear ( loadScreen );
                yield return new WaitForSeconds ( 0.4f );

                loadSquareFiller.gameObject.SetActive ( false );
                makeAllScenesDisable ();
                animateTopdown ( gameModeSettingsScreen );
                loadScreen.GetComponent<Transform> ().localScale = new Vector3 ( 1.0f, 1.0f, 1.0f );
            }

            Generator.Instance.notGenerated = false;
        }

        private IEnumerator makeTransitionToIntroScreen ()
        {
            animateDisappear ( gameModeSettingsScreen );
            yield return new WaitForSeconds ( 0.3f );

            makeAllScenesDisable ();
            animateTopdown ( introScreen );
            gameModeSettingsScreen.GetComponent<Transform> ().localScale = new Vector3 ( 1.0f, 1.0f, 1.0f );
        }

        private IEnumerator makeTransitionToGameModeScreen ( GameObject whereFrom )
        {
            animateDisappear ( whereFrom );
            yield return new WaitForSeconds ( 0.3f );

            if ( whereFrom.name.Equals ( "GameIsOn" ) )
            {
                GamePlay.Instance.questionBoard.SetActive ( true );
                GamePlay.Instance.showHide.SetActive ( true );
            }

            Grid.Instance.destroyGrid ();
            makeAllScenesDisable ();
            animateTopdown ( gameModeSettingsScreen );
            whereFrom.GetComponent<Transform> ().localScale = new Vector3 ( 1.0f, 1.0f, 1.0f );
        }

        private IEnumerator makeTransitionToGameScreen ()
        {
            if(isContinue)
            {
                animateDisappear ( introScreen );
                yield return new WaitForSeconds ( 0.3f );

                makeAllScenesDisable ();
                StartCoroutine ( makeLoadScreen ( gameIsOnScreen ) );
                introScreen.GetComponent<Transform> ().localScale = new Vector3 ( 1.0f, 1.0f, 1.0f );
            }

            else
            {
                animateDisappear ( gameModeSettingsScreen );
                yield return new WaitForSeconds ( 0.3f );

                makeAllScenesDisable ();
                StartCoroutine ( makeLoadScreen ( gameIsOnScreen ) );
                gameModeSettingsScreen.GetComponent<Transform> ().localScale = new Vector3 ( 1.0f, 1.0f, 1.0f );
            }
        }

        void Update ()
        {
            if ( gameObject.activeSelf )
            {
                vane.transform.Rotate ( Vector3.back );
            }
        }
    }
}
