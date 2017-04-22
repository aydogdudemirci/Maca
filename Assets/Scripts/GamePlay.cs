using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace Maca
{
    public class GamePlay : Singleton<GamePlay>
    {
        public GameObject imagePanel;
        public GameObject gamePanel;
        public GameObject question;
        public GameObject questionBoard;
        public GameObject showHide;

        public Color highLight;
        public Color highDark;
        public Color highGarden;

        public Color start;
        public Color end;

        public Color letterBoxColor;
        public Color blackBoxColor;

        public GameObject images;

        Point position;
        public bool isFinished;

        Stack<int> highlightedSlots;

        Stopwatch gameTime;

        List<GameObject> grid
        {
            get
            {
                return Grid.grid;
            }
        }

        Crossword c
        {
            get
            {
                return Generator.crossword;
            }
        }

        private void Awake ()
        {
            instance = this;
        }

        public void createCrossword ()
        {
            Grid.Instance.createGrid ();

            highlightedSlots = new Stack<int> ();

            setPlayingStuff ();
            activateFrame ();
            addImageToFrame ();
            animateFrame ();
            removeHighlights ();
            highlightPosition ();
            fillBoxes ();
            isFinished = false;

            gameTime = new Stopwatch ();
            gameTime.Start ();
        }

        private void fillBoxes ()
        {
            int i=0;

            foreach(GameObject box in grid)
            {
                box.GetComponentInChildren<Text>().text = c.filledSlots[i];
                i++;
            }
        }

        internal void delete ()
        {
            removeHighlights ();

            if (grid[position.index].GetComponentInChildren<Text>().text.Equals(" "))
            {
                position = c.getPreviousSlot (position);
            }

            else
            {
                grid[position.index].GetComponentInChildren<Text> ().text = " ";
                c.filledSlots[position.index] = " ";
            }

            highlightPosition ();
        }

        private void removeHighlights ()
        {
            while ( highlightedSlots.Count > 0 )
            {
                if ( grid[highlightedSlots.Peek ()].tag.Equals ( "BlackBox" ) )
                {
                    grid[highlightedSlots.Pop ()].GetComponent<Image> ().color = blackBoxColor;
                }

                else
                {
                    grid[highlightedSlots.Pop ()].GetComponent<Image> ().color = letterBoxColor;
                }
            }
        }

        internal bool checkEnd ()
        {
            if(ButtonManager.Instance.showHideButton.value == 1.0f)
            {
                return false;
            }

            for(int i=0; i<c.x * c.y; i++)
            {
                if( c.grid[i].Equals(false) && !grid[i].GetComponentInChildren<Text>().text.Equals(c.answers[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public IEnumerator makeEnd()
        {
            foreach ( GameObject box in grid )
            {
                if(!box.tag.Equals("BlackBox"))
                {
                    box.GetComponent<Image> ().color = highLight;
                }
            }

            isFinished = true;

            yield return new WaitForSeconds ( 0.5f );

            questionBoard.SetActive ( false );
            showHide.SetActive ( false );
            images.SetActive ( false );

            while ( Grid.Instance.gameBoard.transform.position != Grid.Instance.reference2.position )
            {
                float step = 25 * Time.deltaTime;
                Grid.Instance.gameBoard.transform.position = Vector3.MoveTowards ( Grid.Instance.gameBoard.transform.position, Grid.Instance.reference2.position, step );
                yield return new WaitForEndOfFrame ();
            }
        }

        public void setPlayingStuff ()
        {
            position = c.grid[c.polePosition + 1] ? new Point ( c.polePosition, false ) : new Point ( c.polePosition, true );

            images = Instantiate ( imagePanel, Vector2.zero, Quaternion.identity ) as GameObject;
            images.transform.SetParent ( gamePanel.transform );
            images.transform.localScale = new Vector3 ( 1.0f, 1.0f );
            images.transform.localPosition = new Vector3 ( 0.0f, -50.0f );

            question.GetComponent<Text> ().text = c.questionsOfAcrossWords[0];
        }

        private void activateFrame ()
        {
            deactivateFrame ();

            if ( !c.isBlackBox(position) )
            {
                images.SetActive ( true );

                question.GetComponent<Text> ().text = c.getQuestion (position);
            }

            else
            {
                images.SetActive ( false );
            }
        }

        public void deactivateFrame ()
        {
            images.SetActive ( false );
            question.GetComponent<Text> ().text = " ";
        }

        private void highlightPosition ()
        {
            if ( c.isBlackBox(position))
            {
                grid[position.index].GetComponent<Image> ().color = highDark;
                highlightedSlots.Push ( position.index );

                question.GetComponent<Text> ().text = "% " + c.blackBoxPercentage.ToString ();
            }

            else
            {
                foreach ( int slot in c.currentWord(position))
                {
                    grid[slot].GetComponent<Image> ().color = highLight;
                    highlightedSlots.Push ( slot );
                }
            }
        }

        public void addImageToFrame ()
        {
            if ( images.activeSelf )
            {
                foreach ( Transform child in images.transform )
                {
                    if ( !child.name.Equals ( "Frame" ) )
                    {
                        child.gameObject.SetActive ( false );
                    }
                }

                images.transform.FindChild ( c.getImageName(position) ).gameObject.SetActive ( true );
            }
        }

        private void animateFrame ()
        {
            activateFrame ();

            if ( GUIManager.Instance.gameIsOnScreen.activeSelf )
            {
                images.GetComponent<Animator> ().SetBool ( "SlideOut", true );
            }
        }

        public void userInteract ( string command )
        {
            removeHighlights ();

            Point previousPosition = new Point(position.index, position.isAcross);

            if ( command.Equals ( "DOWN" ) )
            {
                if ( position.index < c.x * c.y - c.x )
                {
                    position.index += c.x;
                }
            }

            else if ( command.Equals ( "UP" ) )
            {
                if ( position.index > c.x - 1 )
                {
                    position.index -= c.x;
                }
            }

            else if ( command.Equals ( "RIGHT" ) )
            {
                if ( position.index % c.x != c.x - 1 )
                {
                    position.index += 1;
                }
            }

            else if ( command.Equals ( "LEFT" ) )
            {
                if ( position.index % c.x != 0 )
                {
                    position.index -= 1;
                }
            }

            else if ( command.Equals ( "SHIFT" ) )
            {
                position.isAcross = !position.isAcross;
            }

            else if( command.Equals ( "BACKSPACE" ) )
            {
                delete ();
            }

            else if (command.Length == 1)
            {
                write (command);
            }

            decideImagesPanelAction ( previousPosition );

            highlightPosition ();
        }

        private void decideImagesPanelAction ( Point previousPosition )
        {
            if ( c.isBlackBox ( position ) )
            {
                deactivateFrame ();
            }

            else if ( c.isThisOneLetterArea ( position ) )
            {
                deactivateFrame ();
            }

            else if( c.isPassedAnotherWord( previousPosition, position) )
            {
                deactivateFrame ();
                activateFrame ();
            }
        }

        public void write ( string input )
        {
            removeHighlights ();

            if ( !c.isBlackBox(position) )
            {
                grid[position.index].GetComponentInChildren<Text> ().text = input[0].ToString ();
                c.filledSlots[position.index] = input[0].ToString ();

                position = c.getNextPretty ( position );
            }

            else
            {
                deactivateFrame ();
            }

            highlightPosition ();
        }

        private void Update ()
        {
            if(Grid.Instance.isThereGrid )
            {
                if(!grid[position.index].tag.Equals ( "BlackBox" ))
                {
                    grid[position.index].GetComponent<Image> ().color = Color.Lerp ( start, end, Mathf.PingPong ( Time.time, 0.5f ) );
                }

                if ( gameTime.Elapsed.TotalSeconds > 5.0f )
                {
                    StartCoroutine ( checkPoint ());
                }
            }
        }

        IEnumerator checkPoint()
        {
            Generator.Instance.saveCheckPointToDatabase ();

            gameTime.Reset ();
            gameTime.Start ();

            yield return new WaitForEndOfFrame ();
        }
    }
}

