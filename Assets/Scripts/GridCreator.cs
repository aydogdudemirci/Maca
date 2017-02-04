using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Maca
{
    public class GridCreator : Singleton<GridCreator>
    {
        public List<List<GameObject>> grid;
        public List<GameObject> row;

        private XYCouple size;
        private int index;
        private bool isThereGrid;

        private void Awake()
        {
            instance = this;
            isThereGrid = false;
        }

        public void createGrid()
        {
            index = 0;

            if (GUIManager.Instance.isPseudo)
            {
                size = new XYCouple(GUIManager.Instance.X, GUIManager.Instance.Y);
            }

            else
            {
                //get size values from DataManager
            }

            grid = new List<List<GameObject>>();

            for (int i = 0; i < size.y + 1; i++)
            {
                row = new List<GameObject>();

                for (int j = 0; j < size.x + 1; j++)
                {
                    row.Add(instantiateBox(decideBoxType(i, j), i, j));
                }

                grid.Add(row);
            }

            StartCoroutine(calculateProperGridSize(size.x, size.y));
            StartCoroutine(gridAlignmentProperlyOnScreen());

            isThereGrid = true;
        }

        private GameObject decideBoxType(int i, int j)
        {
            GameObject boxType = null;

            if (i == 0 && j == 0)
            {
                boxType = GUIManager.Instance.emptyBox;
            }

            else if (i == 0 || j == 0)
            {
                boxType = GUIManager.Instance.numberBox;
            }

            else
            {
                if (Motor.Instance.puzzleGrid[index] == 0)
                {
                    boxType = GUIManager.Instance.letterBox;
                }

                else
                {
                    boxType = GUIManager.Instance.blackBox;
                }

                index++;
            }

            return boxType;
        }

        private GameObject instantiateBox(GameObject type, int i, int j)
        {
            GameObject aBox = null;

            aBox = Instantiate(type, Vector2.zero, Quaternion.identity) as GameObject;
            aBox.transform.SetParent(GUIManager.Instance.gameBoard.transform);
            aBox.transform.localScale = new Vector3(1.0f, 1.0f);

            if ((i == 0 && j != 0) || (i != 0 && j == 0))
            {
                aBox.GetComponentInChildren<Text>().text = (System.Math.Max(i, j)).ToString();
            }

            return aBox;
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

            GamePlay.Instance.setupGamePlay();
        }

        public void destroyGrid()
        {
            if (isThereGrid)
            {
                for (int i = 0; i < grid.Count; i++)
                {
                    for (int j = 0; j < grid[i].Count; j++)
                    {
                        Destroy(grid[i][j]);
                    }
                }

                for (int i = 0; i < grid.Count; i++)
                {
                    grid[i].Clear();
                }

                grid.Clear();

                isThereGrid = false;
            }
        }
    }
}
