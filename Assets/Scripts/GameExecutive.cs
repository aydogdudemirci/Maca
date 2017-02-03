using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Maca
{
    public class GameExecutive : Singleton<GameExecutive>
    {
        private int index, x, y;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            index = 0;
        }

        public void createGrid()
        {
            if(GUIManager.Instance.isPseudo)
            {
                x = GUIManager.Instance.sizeX;
                y = GUIManager.Instance.sizeY;
            }

            else
            {
                //get size values from DataManager
            }


            instantiateBox(GUIManager.Instance.emptyBox, -1);

            for (int i = 1; i < x + 1; i++)
            {
                instantiateBox(GUIManager.Instance.numberBox, i);
            }

            for (int i = 1; i < y + 1; i++)
            {
                instantiateBox(GUIManager.Instance.numberBox, i);

                for (int j = 0; j < x; j++)
                {
                    if (Motor.Instance.puzzleGrid[index] == 0)
                    {
                        instantiateBox(GUIManager.Instance.letterBox, -1);
                    }

                    else
                    {
                        instantiateBox(GUIManager.Instance.blackBox, -1);
                    }

                    index++;
                }
            }

            StartCoroutine(calculateProperGridSize(x, y));
            StartCoroutine(gridAlignmentProperlyOnScreen());
        }

        private void instantiateBox(GameObject type, int number)
        {
            GameObject aBox = null;

            aBox = Instantiate(type, Vector2.zero, Quaternion.identity) as GameObject;
            aBox.transform.SetParent(GUIManager.Instance.gameBoard.transform);
            aBox.transform.localScale = new Vector3(1.0f, 1.0f);

            if (number != -1)
            {
                aBox.GetComponentInChildren<Text>().text = (number).ToString();
            }

            GUIManager.Instance.boxes.Add(aBox);
        }

        private IEnumerator calculateProperGridSize(int x, int y)
        {
            yield return new WaitForEndOfFrame();

            int max = System.Math.Max(x, y);

            if (max < 11)
            {
                GUIManager.Instance.gameBoard.GetComponent<GridLayoutGroup>().cellSize = new Vector2(80.0f, 80.0f);
                GUIManager.Instance.gameBoard.GetComponent<GridLayoutGroup>().spacing = new Vector2(10.0f, 10.0f);
                GUIManager.Instance.numberBox.GetComponentInChildren<Text>().fontSize = 60;
            }

            else
            {
                GUIManager.Instance.gameBoard.GetComponent<GridLayoutGroup>().cellSize = new Vector2(830.0f / max, 830.0f / max);

                float cellSize = GUIManager.Instance.gameBoard.GetComponent<GridLayoutGroup>().cellSize.x;

                GUIManager.Instance.gameBoard.GetComponent<GridLayoutGroup>().spacing = new Vector2(10 - (80 / cellSize), 10 - (80 / cellSize));
                GUIManager.Instance.numberBox.GetComponentInChildren<Text>().fontSize = 60 - (max - 10) * 5 / 2;
            }

            GUIManager.Instance.gameBoard.GetComponent<GridLayoutGroup>().constraintCount = x + 1;
        }

        private IEnumerator gridAlignmentProperlyOnScreen()
        {
            yield return new WaitForEndOfFrame();

            GUIManager.Instance.gameBoard.transform.position = GUIManager.Instance.reference.position;
        }
    }
}
