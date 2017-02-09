using System;
using UnityEngine;

namespace Maca
{
    public class KeyListener : Singleton<KeyListener>
    {
        enum direction { LEFTTORIGHT, UPTODOWN };

        public string key;

        private void Awake()
        {
            instance = this;
        }

        void Update()
        {
            if (Input.anyKeyDown && GridCreator.Instance.isThereGrid)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    GamePlay.Instance.go("DOWN");
                }

                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    GamePlay.Instance.go("UP");
                }

                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    GamePlay.Instance.go("LEFT");
                }

                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    GamePlay.Instance.go("RIGHT");
                }

                else if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    GamePlay.Instance.delete();   
                }

                else if (Input.GetKeyDown(KeyCode.Return))
                {
                    
                }

                else if (Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift))
                {
                    GamePlay.Instance.changeDirection();
                }

                else if (System.Text.RegularExpressions.Regex.IsMatch(Input.inputString, @"^[\p{L}]+$"))
                {
                    if (Input.inputString == "i" || Input.inputString == "İ")
                    {
                        key = "İ";
                    }

                    else
                    {
                        key = Input.inputString.ToUpper();
                    }

                    GamePlay.Instance.write(key);
                }

                else if(System.Text.RegularExpressions.Regex.IsMatch(Input.inputString, @"^[\p{N}]+$"))
                {
                    key = Input.inputString;
                }

                //else
                //{
                //    GamePlay.Instance.invalidInput();
                //}
            }
        }  
    }
}