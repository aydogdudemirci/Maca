using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Maca
{
    public class GamePlay : Singleton<GamePlay>
    {
        Color highlightLight;
        Color highlightDark;
        //Color highlightRed;

        Color letterBox;
        Color blackBox;

        XYCouple size;

        public bool isFromLeftToRight;
        private GameObject images;

        int x;
        int y;

        List<List<GameObject>> grid
        {
            get
            {
                return GridCreator.Instance.grid;
            }
        }

        private void Awake()
        {
            instance = this;
        }

        public void setupGamePlay()
        {
            x = 1;
            y = 1;

            isFromLeftToRight = true;

            if (GUIManager.Instance.isPseudo)
            {
                size = new XYCouple((int)ButtonManager.Instance.sliders[2].value * 2, (int)ButtonManager.Instance.sliders[3].value * 2);
            }

            else
            {
                //get size from settings
            }

            letterBox = GUIManager.Instance.letterBox.GetComponent<Image>().color;
            blackBox = GUIManager.Instance.blackBox.GetComponent<Image>().color;

            highlightLight = GUIManager.Instance.highlightLight;
            highlightDark = GUIManager.Instance.highlightDark;
            //highlightRed = GUIManager.Instance.highlightRed;

            highlightBox();
        }

        public void go(string keyword)
        {
            removeHighlightBox();

            if (keyword.Equals("DOWN"))
            {
                if (y < size.y)
                {
                    y += 1;
                }
            }

            else if(keyword.Equals("UP"))
            {
                if (y > 1)
                {
                    y -= 1;
                }
            }

            else if (keyword.Equals("RIGHT"))
            {
                if (x < size.x)
                {
                    x += 1;
                }
            }

            else if (keyword.Equals("LEFT"))
            {
                if (x > 1)
                {
                    x -= 1;
                }
            }

            highlightBox();
        }

        public void write(string keyword)
        {
            removeHighlightBox();

            if (grid[y][x].tag.Equals("LetterBox"))
            {
                grid[y][x].GetComponentInChildren<Text>().text = keyword[0].ToString();

                if (isFromLeftToRight)
                {
                    if (x < size.x && grid[y][x + 1].tag.Equals("LetterBox"))
                    {
                        continueWord();
                    }

                    else
                    {
                        concludeWord();
                    }
                }

                else
                {
                    if (y < size.y && grid[y + 1][x].tag.Equals("LetterBox"))
                    {
                        continueWord();
                    }

                    else
                    {
                        concludeWord();
                    }
                }
            }

            else
            {
                if (isFromLeftToRight && x < size.x)
                {
                    x += 1;
                }

                else if(!isFromLeftToRight && y < size.y)
                {
                    y += 1;
                }
            }

            highlightBox();
        }

        public void highlightBox()
        {
            updateQuestion();

            if (grid[y][x].tag.Equals("LetterBox"))
            {
                grid[y][x].GetComponent<Image>().color = highlightLight;
            }

            else
            {
                grid[y][x].GetComponent<Image>().color = highlightDark;
            }

            if(isFromLeftToRight)
            {
                grid[y][x].transform.GetChild(0).gameObject.SetActive(true);
                grid[y][x].transform.GetChild(1).gameObject.SetActive(false);
            }

            else
            {
                grid[y][x].transform.GetChild(0).gameObject.SetActive(false);
                grid[y][x].transform.GetChild(1).gameObject.SetActive(true);
            }
        }

        public void removeHighlightBox()
        {
            removeQuestion();

            if (grid[y][x].tag.Equals("LetterBox"))
            {
                grid[y][x].GetComponent<Image>().color = letterBox;
            }

            else
            {
                grid[y][x].GetComponent<Image>().color = blackBox;
            }

            grid[y][x].transform.GetChild(0).gameObject.SetActive(false);
            grid[y][x].transform.GetChild(1).gameObject.SetActive(false);
        }

        public void concludeWord()
        {
            if (isFromLeftToRight)
            {
                if (y < size.y && grid[y + 1][x].tag.Equals("LetterBox"))
                {
                    y += 1;
                    changeDirection();
                }
            }

            else
            {
                if (x < size.x && grid[y][x + 1].tag.Equals("LetterBox"))
                {
                    x += 1;
                    changeDirection();
                }
            }
        }

        public void continueWord()
        {
            if (isFromLeftToRight)
            {
                if(grid[y][x + 1].GetComponentInChildren<Text>().text.Equals(""))
                {
                    x += 1;
                }

                else
                {
                    while(size.x > x && !grid[y][x + 1].tag.Equals("BlackBox") && !grid[y][x].GetComponentInChildren<Text>().text.Equals(""))
                    {
                        x += 1;
                    }
                }

                if (x == size.x || grid[y][x + 1].tag.Equals("BlackBox"))
                {
                    changeDirection();
                }
            }

            else
            {
                if (grid[y + 1][x].GetComponentInChildren<Text>().text.Equals(""))
                {
                    y += 1;
                }

                else
                {
                    while (size.y > y && !grid[y + 1][x].tag.Equals("BlackBox") && !grid[y][x].GetComponentInChildren<Text>().text.Equals(""))
                    {
                        y += 1;
                    }
                }

                if (y == size.y || grid[y + 1][x].tag.Equals("BlackBox"))
                {
                    changeDirection();
                }
            }
        }

        public void changeDirection()
        {
            removeHighlightBox();
            isFromLeftToRight = !isFromLeftToRight;
            highlightBox();
        }

        public void delete()
        {
            removeHighlightBox();

            if (isFromLeftToRight)
            {
                if (grid[y][x].tag.Equals("LetterBox"))
                {
                    if ( !grid[y][x].GetComponentInChildren<Text>().text.Equals("") )
                    {
                        grid[y][x].GetComponentInChildren<Text>().text = "";
                    }

                    else if ( x > 1 && grid[y][x - 1].tag.Equals("LetterBox"))
                    {
                        x -= 1;
                    }
                }
            }

            else
            {
                if (grid[y][x].tag.Equals("LetterBox"))
                {
                    if (!grid[y][x].GetComponentInChildren<Text>().text.Equals(""))
                    {
                        grid[y][x].GetComponentInChildren<Text>().text = "";
                    }

                    else if (y > 1 && grid[y - 1][x].tag.Equals("LetterBox"))
                    {
                        y -= 1;
                    }
                }
            }

            highlightBox();
        }

        private void removeQuestion()
        {
            if (grid[y][x].tag.Equals("LetterBox"))
            {
                GUIManager.Instance.question.GetComponent<Text>().text = "";
            }

            if (Motor.Instance.getImageNumber(x, y, isFromLeftToRight) != 0)
            {
                Destroy(images);
            }
        }

        private void updateQuestion()
        {
            if (grid[y][x].tag.Equals("LetterBox"))
            {
                GUIManager.Instance.question.GetComponent<Text>().text = Motor.Instance.getQuestion(x, y, isFromLeftToRight);
            }

            if (Motor.Instance.getImageNumber(x, y, isFromLeftToRight) != 0)
            {
                images = null;

                images = Instantiate(GUIManager.Instance.images, Vector2.zero, Quaternion.identity) as GameObject;
                images.transform.SetParent(GUIManager.Instance.gamePanel.transform);
                images.transform.localScale = new Vector3(1.0f, 1.0f);
                images.transform.localPosition = new Vector3(0.0f, 0.0f);

                images.SetActive(true);
                images.transform.GetChild(Motor.Instance.getImageNumber(x, y, isFromLeftToRight) - 1).gameObject.SetActive(true);
            }
        }

        private void FixedUpdate()
        {
            if(GridCreator.Instance.isThereGrid && x > 0 && y > 0)
            {
                if (isFromLeftToRight)
                {
                    if (grid[y][x].transform.GetChild(0).gameObject.activeSelf)
                    {
                        grid[y][x].transform.GetChild(0).gameObject.SetActive(false);
                    }

                    else
                    {
                        grid[y][x].transform.GetChild(0).gameObject.SetActive(true);
                    }
                }

                else
                {
                    if (grid[y][x].transform.GetChild(1).gameObject.activeSelf)
                    {
                        grid[y][x].transform.GetChild(1).gameObject.SetActive(false);
                    }

                    else
                    {
                        grid[y][x].transform.GetChild(1).gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
