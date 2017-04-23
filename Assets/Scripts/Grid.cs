using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Maca
{
    public class Grid : Singleton<Grid>
    {
        public GameObject emptyBox;
        public GameObject numberBox;
        public GameObject letterBox;
        public GameObject blackBox;

        public GameObject gameBoard;
        public Transform reference;
        public Transform reference2;

        public static List<GameObject> grid;
        public List<GameObject> others;

        public Vector2 cellSize;
        public Vector2 spacing;

        public bool isThereGrid;

        Crossword crossword
        {
            get
            {
                return GamePlay.Instance.c;
            }
        }

        private void Awake ()
        {
            instance = this;
            isThereGrid = false;
        }

        public void createGrid ()
        {
            grid = new List<GameObject> ();
            others = new List<GameObject> ();

            for ( int i = 0; i < crossword.y + 1; i++ )
            {
                for ( int j = 0; j < crossword.x + 1; j++ )
                {
                    instantiate ( type ( j, i ), j, i );
                }
            }

            defineGridProperties ();
            alignGridOnScreen ();

            isThereGrid = true;
        }

        private void instantiate ( GameObject type, int i, int j )
        {
            GameObject newBox = null;

            newBox = Instantiate ( type, Vector2.zero, Quaternion.identity ) as GameObject;
            newBox.transform.SetParent ( gameBoard.transform );
            newBox.transform.localScale = new Vector3 ( 1.0f, 1.0f );

            if ( ( i == 0 && j != 0 ) || ( i != 0 && j == 0 ) )
            {
                newBox.GetComponentInChildren<Text> ().text = i == 0 ? j.ToString () : i.ToString (); 
            }

            if ( type.tag.Equals ( "LetterBox" ) || type.tag.Equals ( "BlackBox" ) )
            {
                grid.Add ( newBox );
            }

            else
            {
                others.Add ( newBox );
            }
        }

        private void defineGridProperties ()
        {
            int fontSize;
            int max = crossword.x >= crossword.y ? crossword.x : crossword.y;

            if ( max < 11 )
            {
                fontSize = 60;

                cellSize = new Vector2 ( 80.0f, 80.0f );
                spacing = new Vector2 ( 10.0f, 10.0f );
            }

            else
            {
                fontSize = 60 - ( max - 10 ) * 5 / 2;

                cellSize = new Vector2 ( 830.0f / max, 830.0f / max );
                spacing = new Vector2 ( 10 - ( 80 / cellSize.x ), 10 - ( 80 / cellSize.y ) );
            }

            foreach ( GameObject box in grid )
            {
                box.GetComponentInChildren<Text> ().fontSize = fontSize;
            }

            foreach ( GameObject box in others )
            {
                box.GetComponentInChildren<Text> ().fontSize = fontSize;
            }

            gameBoard.GetComponent<GridLayoutGroup> ().cellSize = cellSize;
            gameBoard.GetComponent<GridLayoutGroup> ().spacing = spacing;
            gameBoard.GetComponent<GridLayoutGroup> ().constraintCount = crossword.x + 1;
        }

        private void alignGridOnScreen ()
        {
            gameBoard.transform.position = reference.position;
        }

        public void destroyGrid ()
        {
            if ( isThereGrid )
            {
                GamePlay.Instance.deactivateFrame ();

                foreach ( GameObject box in grid )
                {
                    Destroy ( box );
                }

                grid.Clear ();

                foreach ( GameObject box in others )
                {
                    Destroy ( box );
                }

                others.Clear ();

                ButtonManager.Instance.showHideButton.value = 0;
                Destroy ( GamePlay.Instance.images );

                isThereGrid = false;
            }
        }

        private GameObject type ( int i, int j )
        {
            GameObject box = null;

            if ( i == 0 && j == 0 )
            {
                box = emptyBox;
            }

            else if ( i == 0 || j == 0 )
            {
                box = numberBox;
            }

            else
            {
                i -= 1;
                j -= 1;

                box = crossword.grid[(j * crossword.x) + i] ? blackBox : letterBox;
            }

            return box;
        }
    }
}
