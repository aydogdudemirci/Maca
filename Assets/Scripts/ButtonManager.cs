using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Maca
{
    public class ButtonManager : Singleton<ButtonManager>
    {
        public List<Slider> sliders;
        public List<GameObject> infos;

        public Slider showHideButton;
        public Slider emergency;

        public Color highlighted;
        public Color disabled;

        Crossword crossword
        {
            get
            {
                return Generator.crossword;
            }
        }

        List<GameObject> grid
        {
            get
            {
                return Grid.grid;
            }
        }
        private void Awake ()
        {
            instance = this;
        }

        private void Start ()
        {
            Generator.Instance.getPreferences ();
            highlight ();
        }

        public void colored ()
        {
            foreach ( GameObject info in infos )
            {
                foreach ( Transform inf in info.transform )
                {
                    inf.GetComponent<Text> ().color = disabled;
                }
            }

            highlight ();
        }

        private void highlight ()
        {
            infos[0].transform.GetChild ( ( int ) sliders[0].value - 1 ).GetComponent<Text> ().color = highlighted;
            infos[1].transform.GetChild ( ( int ) sliders[1].value - 1 ).GetComponent<Text> ().color = highlighted;
            infos[2].transform.GetChild ( ( int ) sliders[2].value - 5 ).GetComponent<Text> ().color = highlighted;
            infos[2].transform.GetChild ( ( int ) sliders[3].value - 5 ).GetComponent<Text> ().color = highlighted;
        }

        public void showHide ()
        {
            if ( showHideButton.value == 1.0f )
            {
                for ( int i = 0; i < grid.Count; i++ )
                {
                    if ( grid[i].tag.Equals ( "LetterBox" ) )
                    {
                        grid[i].GetComponentInChildren<Text> ().text = crossword.answers[i];
                    }
                }
            }

            else
            {
                for ( int i = 0; i < grid.Count; i++ )
                {
                    if ( grid[i].tag.Equals ( "LetterBox" ) )
                    {
                        grid[i].GetComponentInChildren<Text> ().text = crossword.filledSlots[i];
                    }
                }
            }
        }
    }
}