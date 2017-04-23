using UnityEngine;

namespace Maca
{
    public class KeyListener : Singleton<KeyListener>
    {
        public string key;

        private void Awake ()
        {
            instance = this;
        }

        void Update ()
        {
            if ( Input.anyKeyDown && Grid.Instance.isThereGrid && !GamePlay.Instance.checkEnd () )
            {
                BackgroundGenerator.Instance.processDecision ();

                if ( Input.GetKeyDown ( KeyCode.DownArrow ) )
                {
                    GamePlay.Instance.userInteract ( "DOWN" );
                }

                else if ( Input.GetKeyDown ( KeyCode.UpArrow ) )
                {
                    GamePlay.Instance.userInteract ( "UP" );
                }

                else if ( Input.GetKeyDown ( KeyCode.LeftArrow ) )
                {
                    GamePlay.Instance.userInteract ( "LEFT" );
                }

                else if ( Input.GetKeyDown ( KeyCode.RightArrow ) )
                {
                    GamePlay.Instance.userInteract ( "RIGHT" );
                }

                else if ( Input.GetKeyDown ( KeyCode.Backspace ) && ButtonManager.Instance.showHideButton.value == 0f && !GamePlay.Instance.isFinished )
                {
                    GamePlay.Instance.userInteract ( "BACKSPACE" );
                }

                //else if ( Input.GetKeyDown ( KeyCode.Return ) )
                //{

                //}

                else if ( Input.GetKeyDown ( KeyCode.RightShift ) || Input.GetKeyDown ( KeyCode.LeftShift ) )
                {
                    GamePlay.Instance.userInteract ( "SHIFT" );
                }

                else if ( System.Text.RegularExpressions.Regex.IsMatch ( Input.inputString, @"^[\p{L}]+$" ) && ButtonManager.Instance.showHideButton.value == 0f && !GamePlay.Instance.isFinished )
                {
                    if ( Input.inputString == "i" || Input.inputString == "İ" )
                    {
                        key = "İ";
                    }

                    else
                    {
                        key = Input.inputString.ToUpper ();
                    }

                    GamePlay.Instance.userInteract ( key );
                }

                if( GamePlay.Instance.checkEnd ())
                {
                    StartCoroutine ( GamePlay.Instance.makeEnd ());
                }
            }
        }
    }
}