﻿using UnityEngine;

namespace Maca
{
    public class KeyListener : Singleton<KeyListener>
    {
        enum direction { right, down };
        enum tour { LEFT, RIGHT, UP, DOWN };

        public string key;

        private void Awake()
        {
            instance = this;
        }

        void OnGUI()
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    go(tour.DOWN);
                }

                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    go(tour.UP);
                }

                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    go(tour.LEFT);
                }

                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    go(tour.RIGHT);
                }

                else if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    delete();
                }

                else if (Input.GetKeyDown(KeyCode.Return))
                {
                    enter();
                }

                else if (Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift))
                {
                    shift();
                }

                else if (System.Text.RegularExpressions.Regex.IsMatch(Input.inputString, @"^[a-zA-ZıIğĞüÜşŞiİöÖçÇ]+$"))
                {
                    if (Input.inputString == "i" || Input.inputString == "İ")
                    {
                        key = "İ";
                    }

                    else
                    {
                        key = Input.inputString.ToUpper();
                    }

                    write(key);
                }

                else if(System.Text.RegularExpressions.Regex.IsMatch(Input.inputString, @"^[0-9]+$"))
                {
                    key = Input.inputString;

                    goToCell(key);
                }
            }
        }

        private void go(tour request)
        {

        }

        private void delete()
        {

        }

        private void enter()
        {

        }

        private void shift()
        {

        }

        private void write(string key)
        {

        }

        private void goToCell(string num)
        {

        }
    }
}