using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Maca
{
    public class GamePlay : Singleton<GamePlay>
    {
        private int currentBox;

        private void Awake()
        {
            instance = this;
        }

        public void setupGamePlay()
        {
            currentBox = 0;
        }

        public void go(string keyword)
        {
            if(keyword.Equals("DOWN"))
            {

            }
        }

        public void highlightCurrentBox(int newCurrentBox)
        {

        }
    }
}
