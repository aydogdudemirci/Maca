using System.Collections.Generic;

namespace Maca
{
    public class Crossword
    {
        public int x;
        public int y;
        public int polePosition;
        public int difficulty;
        public int blackBoxPercentage;

        public string creationTime;
        public string category;

        public string creationDate;

        public List<string> filledSlots;
        public List<string> questionsOfAcrossWords; 
        public List<string> questionsOfDownWords;
        public List<string> answers; 
        public List<string> acrossWords; 
        public List<string> downWords; 

        public List<int> imagesOfAcrossWords; 
        public List<int> imagesOfDownWords; 
        public List<int> acrossID; 
        public List<int> downID;

        public List<List<int>> indexesOfAcrossWords;
        public List<List<int>> indexesOfDownWords;

        public List<bool> grid;

        public Crossword ()
        {
            grid = new List<bool> ();
            acrossID = new List<int> ();
            downID = new List<int> ();
            filledSlots = new List<string> ();
            questionsOfAcrossWords = new List<string> ();
            questionsOfDownWords = new List<string> ();
            answers = new List<string> ();
            imagesOfAcrossWords = new List<int> ();
            imagesOfDownWords = new List<int> ();
            acrossWords = new List<string> ();
            downWords = new List<string> ();

            indexesOfAcrossWords = new List<List<int>>();
            indexesOfDownWords = new List<List<int>> ();
        }

        internal Point getPreviousSlot ( Point p )
        {
            return p.isAcross && p.index % x != 0 && !grid[p.index - 1] ? new Point ( p.index - 1, p.isAcross ) : !p.isAcross && p.index >= x && !grid[p.index - x] ? new Point ( p.index - x, p.isAcross ) : p;
        }

        internal int getIndexOfLastSlotOfCurrentWord(Point p)
        {
            return p.isAcross ? indexesOfAcrossWords[acrossID[p.index]][indexesOfAcrossWords[acrossID[p.index]].Count - 1] : indexesOfDownWords[downID[p.index]][indexesOfDownWords[downID[p.index]].Count - 1];
        }

        internal Point getNextPrettySlot(Point p)
        {
            int next = p.isAcross ? 1 : x;

            if((p.isAcross && p.index % x != x - 1) || ( !p.isAcross && p.index < x * y - x ) )
            {
                for ( int i = p.index + next; i <= getIndexOfLastSlotOfCurrentWord ( p ); i += next )
                {
                    if ( filledSlots[i].Equals ( " " ) )
                    {
                        return new Point(i, p.isAcross);
                    }
                }
            }

            return p;
        }

        internal bool isBlackBox(Point p)
        {
            return grid[p.index];
        }

        internal bool isThisOneLetterArea ( Point p )
        {
            return p.isAcross ? indexesOfAcrossWords[acrossID[p.index]].Count == 1 : indexesOfDownWords[downID[p.index]].Count == 1;
        }

        internal string getQuestion ( Point p )
        {
            return p.isAcross ? questionsOfAcrossWords[acrossID[p.index]] : questionsOfDownWords[downID[p.index]];
        }

        internal bool isPassedAnotherWord ( Point old, Point now )
        {
            return !old.isAcross.Equals(now.isAcross) ? true : now.isAcross && acrossID[now.index] != acrossID[old.index] ? true : !now.isAcross && downID[now.index] != downID[old.index] ? true : false;
        }

        internal string getImageName (Point p)
        {
            return p.isAcross ? imagesOfAcrossWords[acrossID[p.index]].ToString () : imagesOfDownWords[downID[p.index]].ToString ();
        }

        internal List<int> currentWord (Point p)
        {
            return p.isAcross ? indexesOfAcrossWords[acrossID[p.index]] : indexesOfDownWords[downID[p.index]];
        }
    }
}