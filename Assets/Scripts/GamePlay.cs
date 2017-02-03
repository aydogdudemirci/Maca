using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Maca
{
    public class GamePlay : Singleton<GamePlay>
    {
        Color highlightLight;
        Color highlightDark;

        Color letterBox;
        Color blackBox;

        XYCouple box;
        XYCouple size;

        private void Awake()
        {
            instance = this;
        }

        public void setupGamePlay()
        {
            box = new XYCouple(1,1);

            if (GUIManager.Instance.isPseudo)
            {
                size = new XYCouple(GUIManager.Instance.X, GUIManager.Instance.Y);
            }

            else
            {
                //get size from settings
            }

            letterBox = GUIManager.Instance.letterBox.GetComponent<Image>().color;
            blackBox = GUIManager.Instance.blackBox.GetComponent<Image>().color;

            highlightLight = GUIManager.Instance.highlightLight;
            highlightDark = GUIManager.Instance.highlightDark;

            highlightBox();
        }

        public void go(string keyword)
        {
            removeHighlightBox();

            if (keyword.Equals("DOWN"))
            {
                if (box.y < size.y)
                {
                    box.y += 1;
                }
            }

            else if(keyword.Equals("UP"))
            {
                if (box.y > 1)
                {
                    box.y -= 1;
                }
            }

            else if (keyword.Equals("RIGHT"))
            {
                if (box.x < size.x)
                {
                    box.x += 1;
                }
            }

            else if (keyword.Equals("LEFT"))
            {
                if (box.x > 1)
                {
                    box.x -= 1;
                }
            }

            highlightBox();
        }

        public void write(string keyword)
        {

        }

        public void highlightBox()
        {
            if (GridCreator.Instance.grid[box.y][box.x].tag.Equals("LetterBox"))
            {
                GridCreator.Instance.grid[box.y][box.x].GetComponent<Image>().color = highlightLight;
            }

            else
            {
                GridCreator.Instance.grid[box.y][box.x].GetComponent<Image>().color = highlightDark;
            }
                
        }

        public void removeHighlightBox()
        {
            if (GridCreator.Instance.grid[box.y][box.x].tag.Equals("LetterBox"))
            {
                GridCreator.Instance.grid[box.y][box.x].GetComponent<Image>().color = letterBox;
            }

            else
            {
                GridCreator.Instance.grid[box.y][box.x].GetComponent<Image>().color = blackBox;
            }
        }
    }
}
