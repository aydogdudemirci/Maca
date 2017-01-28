using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Maca;

enum direction { right, down };

public class GameExecutive : MonoBehaviour
{
    public GameObject letterBox;
    public GameObject numberBox;
    public GameObject blackBox;
    public GameObject emptyBox;

    public GameObject gamePanel;
    public GameObject gameBoard;

    public List<GameObject> boxes = new List<GameObject>();
   
    int x, y;   //crossword sizes
    int index;

    private void Awake()
    {
        index = 0;

        if (GUIManager.isPseudo)
        {
            x = 10;
            y = 10;
        }

        else
        {
            //Get size from settings
        }

        calculateProperGridValues(x, y);
    }

    private void Start()
    {
        createGrid();
    }
    
    private void createGrid()
    {
        Motor puzzle = gamePanel.GetComponent<Motor>();

        instantiateBox(emptyBox, -1);

        for (int i = 1; i < x + 1; i++)
        {
            instantiateBox(numberBox, i);
        }

        for (int i = 1; i < y + 1; i++)
        {
            instantiateBox(numberBox, i);

            for (int j = 0; j < x; j++)
            {
                if (puzzle.puzzleGrid[index] == 0)
                {
                    instantiateBox(letterBox, -1);
                }

                else
                {
                    instantiateBox(blackBox, -1);
                }

                index++;
            }
        }
    }

    private void instantiateBox(GameObject type, int number)
    {
        GameObject aBox = null;

        aBox = Instantiate(type, Vector2.zero, Quaternion.identity) as GameObject;
        aBox.transform.SetParent(gameBoard.transform);
        aBox.transform.localScale = new Vector3(1.0f, 1.0f);

        if(number != -1)
        {
            aBox.GetComponentInChildren<Text>().text = (number).ToString();
        }

        boxes.Add(aBox);
    }

    private void calculateProperGridValues(int x, int y)
    {
        int max = System.Math.Max(x, y);

        if(max < 11)
        {
            gameBoard.GetComponent<GridLayoutGroup>().cellSize = new Vector2(80.0f, 80.0f);
            numberBox.GetComponentInChildren<Text>().fontSize = 60;
        }

        else
        {
            gameBoard.GetComponent<GridLayoutGroup>().cellSize = new Vector2(750.0f / max, 750.0f / max);
            numberBox.GetComponentInChildren<Text>().fontSize = 60 - (max - 10) * 5 / 2;
        }

        if(x < 11)
        {
            gameBoard.GetComponent<GridLayoutGroup>().padding.left = 750 + (10 - x) * 50;
        }

        else
        {
            gameBoard.GetComponent<GridLayoutGroup>().padding.left = 750;
        }

        if(y < 11)
        {
            gameBoard.GetComponent<GridLayoutGroup>().padding.top = 50 + (10 - y) * 40;
        }

        else
        {
            gameBoard.GetComponent<GridLayoutGroup>().padding.top = 50;
        }

        gameBoard.GetComponent<GridLayoutGroup>().constraintCount = x + 1;
    }
}
