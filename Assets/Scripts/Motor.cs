using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maca
{
    public class XYCouple
    {
        public int x;
        public int y;
        public bool direction;

        public XYCouple(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public XYCouple(int x, int y, bool direction)
        {
            this.x = x;
            this.y = y;
            this.direction = direction;
        }
    }

    public class Motor : Singleton<Motor>
    {
        public List<int> puzzleGrid;
        public List<int> questionsTroughRight;
        public List<int> questionsTroughDown;
        public List<string> answers;

        public List<string> questions;
        public List<int> imageNumber;

        void Awake()
        {
            instance = this;
        }

        public void createPuzzle()
        {
            string preferences;
            string[] data = new string[1000];

            preferences = ButtonManager.Instance.sliders[0].value.ToString() + " " +
                          ButtonManager.Instance.sliders[1].value.ToString() + " " +
                          ButtonManager.Instance.sliders[2].value.ToString() + " " +
                          ButtonManager.Instance.sliders[3].value.ToString();

            Networking.Instance.Connect(preferences);
            data = Networking.Instance.puzzleData.ToString().Split('#');

            int x = Convert.ToInt32(data[0]);
            int y = Convert.ToInt32(data[1]);
            int numberOfQuestions = Convert.ToInt32(data[2]);

            int index = 3;

            for (int i = 0; i < x * y; i++ )
            {
                puzzleGrid.Add(Convert.ToInt32(data[index]));
                index++;
            }

            for (int i = 0; i < x * y; i++)
            {
                questionsTroughRight.Add(Convert.ToInt32(data[index]));
                index++;
            }

            for (int i = 0; i < x * y; i++)
            {
                questionsTroughDown.Add(Convert.ToInt32(data[index]));
                index++;
            }

            for (int i = 0; i < numberOfQuestions; i++)
            {
                questions.Add(data[index]);
                index++;
            }

            for (int i = 0; i < numberOfQuestions; i++)
            {
                imageNumber.Add(Convert.ToInt32(data[index]));
                index++;
            }

            for (int i = 0; i < numberOfQuestions; i++)
            {
                answers.Add(data[index]);
                index++;
            }
        }

        public string getQuestion(int x, int y, bool isFromLeftToRight)
        {
            if (isFromLeftToRight)
            {
                return questions[ questionsTroughRight[(y - 1) * GridCreator.Instance.size.x + x - 1] ];
            }

            else
            {
                return questions[ questionsTroughDown[(y - 1) * GridCreator.Instance.size.x + x - 1] ];
            }
        }

        public int getImageNumber(int x, int y, bool isFromLeftToRight)
        {
            if (isFromLeftToRight)
            {
                return imageNumber[questionsTroughRight[(y - 1) * GridCreator.Instance.size.x + x - 1]];
            }

            else
            {
                return imageNumber[questionsTroughDown[(y - 1) * GridCreator.Instance.size.x + x - 1]];
            }
        }

        public GameObject getBoxType(int index)
        {
            if (puzzleGrid[index] == 0)
            {
                return GUIManager.Instance.letterBox;
            }

            else
            {
                return GUIManager.Instance.blackBox;
            }
        }
    }
}