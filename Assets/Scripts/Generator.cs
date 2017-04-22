using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using System.Diagnostics;
using UnityEngine.UI;

namespace Maca
{
    public class Generator : Singleton<Generator>
    {
        public List<Slider> sliders;
        public Slider timeout;

        public static Crossword crossword;

        List<List<string>> letters;
        List<IDataReader> potentials;
        List<string> puzzle;

        List<List<int>> acrossWords;
        List<List<int>> queryWords;
        List<List<int>> downWords;

        List<bool> freeSlots;
        List<int> letterIDs;

        IDbConnection dbConnection;
        IDbCommand dbcmd;
        IDataReader reader;

        public Point size;

        string difficulty;
        int lastUsefulBox;

        public Stopwatch timer;

        bool isEmergency;
        public double emergency;
        public bool isComplete;
        public bool reversing;

        static int lifeCounter;

        private void Awake ()
        {
            instance = this;
            isEmergency = false;
            isComplete = false;
            reversing = false;
        }

        public void generateCrossword ()
        {
            crossword = new Crossword ();

            timer = new Stopwatch ();

            emergency = !isEmergency ? 3.75f : 2.75f;

            if(!reversing)
            {
                createPuzzleGrid ();
            }

            determineFreeSlots ();

            determineSlotsOfAcrossWordsBasedOnPuzzleGrid ();

            determineSlotsOfDownWordsBasedOnPuzzleGrid ();

            determineQueryWordsForEachSlot ();

            createUsefulObjects ();

            timer.Start ();

            if ( !GUIManager.isContinue && !GUIManager.isTimeout && !reversing )
            {
                backtracking ( 0 );
            }

            timer.Stop ();

            createCrosswordAndCollectGarbage ( timer );

            if ( (GUIManager.isContinue || GUIManager.isTimeout) && !reversing )
            {
                isComplete = true;
            }
        }

        private bool backtracking ( int position )
        {
            if ( !puzzle[lastUsefulBox].Equals ( " " ) || timer.Elapsed.TotalSeconds > emergency || GUIManager.totalTime.Elapsed.TotalSeconds > timeout.value )
            {
                isEmergency = timer.Elapsed.TotalSeconds > emergency ? true : false;
                GUIManager.isTimeout = GUIManager.totalTime.Elapsed.TotalSeconds > timeout.value ? true : false;

                UnityEngine.Debug.Log ( timer.Elapsed.TotalSeconds );

                return true;
            }

            findPotentials ( position );

            while ( potentials[position].Read () )
            {
                place ( potentials[position].GetString ( 0 ), position );

                if ( backtracking ( position + 1 ) == true )
                {
                    return true;
                }
            }

            place ( new string ( ' ', acrossWords[position].Count ), position );

            return false;
        }

        private void place ( string word, int position )
        {
            for ( int i = 0; i < acrossWords[position].Count; i++ )
            {
                puzzle[acrossWords[position][i]] = word[i].ToString ();
            }
        }

        public void reverseCrossword ()
        {
            List<string> temp = new List<string>();

            foreach ( string slot in crossword.answers )
            {
                temp.Add ( slot );
            }

            puzzle.Clear ();

            for ( int i = 0; i < getNormalSize ().x; i++ )
            {
                for ( int j = 0; j < getNormalSize ().y; j++ )
                {
                    puzzle.Add (temp[getNormalSize ().x * j + i] );
                }
            }

            size = new Point ( getSize().x, getSize().y );

            reversing = true;

            generateCrossword ();

            reversing = false;
        }

        private void determineFreeSlots ()
        {
            freeSlots = new List<bool> ();

            int i = 0;

            foreach ( string slot in puzzle )
            {
                if ( i < size.x || puzzle[i].Equals ( "1" ) || puzzle[i - size.x].Equals ( "1" ) )
                {
                    freeSlots.Add ( true );
                }

                else
                {
                    freeSlots.Add ( false );
                }

                i++;
            }
        }

        private void findPotentials ( int position )
        {
            potentials[position].Close ();

            string query;

            if ( updateLetters ( position ) )
            {
                query = string.Format ( "SELECT word FROM w{0} WHERE (", acrossWords[position].Count );

                for ( int i = 0; i < acrossWords[position].Count; i++ )
                {
                    foreach ( string letter in letters[acrossWords[position][i]] )
                    {
                        query += string.Format ( " word LIKE '{0}{1}{2}' OR", new string ( '_', i ), letter, new string ( '_', acrossWords[position].Count - i - 1 ) );
                    }

                    query = query.Remove ( query.Length - 3 );
                    query += ") AND (";
                }

                query = query.Remove ( query.Length - 5 );

                if ( position == 0 || position == 1 )
                {
                    query += " ORDER BY RANDOM()";
                }

                else
                {
                    query += difficulty;
                }
            }

            else
            {
                query = "SELECT word FROM w0";
            }

            dbcmd = dbConnection.CreateCommand ();
            dbcmd.CommandText = query;
            potentials[position] = dbcmd.ExecuteReader ();
            dbcmd.Dispose ();
        }

        private bool updateLetters ( int position )
        {
            foreach ( int letterIndex in acrossWords[position] )
            {
                letters[letterIndex].Clear ();
            }

            foreach ( int letterIndex in acrossWords[position] )
            {
                if ( isFree ( letterIndex ) )
                {
                    letters[letterIndex].Add ( "_" );
                }

                else
                {
                    string query = "";

                    foreach ( int letter in queryWords[letterIndex] )
                    {
                        query += puzzle[letter];
                    }

                    query = query.Replace ( ' ', '_' );

                    query = string.Format ( "SELECT DISTINCT SUBSTR(word, {0}, 1) FROM w{1} WHERE word LIKE '{2}'", letterIDs[letterIndex] + 1, query.Length, query );

                    dbcmd = dbConnection.CreateCommand ();
                    dbcmd.CommandText = query;
                    reader = dbcmd.ExecuteReader ();
                    dbcmd.Dispose ();

                    if ( !reader.IsDBNull ( 0 ) )
                    {
                        while ( reader.Read () )
                        {
                            letters[letterIndex].Add ( reader.GetString ( 0 ) );
                        }

                        reader.Close ();
                    }

                    else
                    {
                        reader.Close ();

                        return false;
                    }
                }
            }

            return true;
        }

        private bool isFree ( int letterIndex )
        {
            return freeSlots[letterIndex];
        }

        //public void setDatabaseConnection ()
        //{
        //    string filepath = Application.persistentDataPath + "/" + "words.db";

        //    if ( !File.Exists ( filepath ) )

        //    {

        //        if it doesn't ->

        //         open StreamingAssets directory and load the db->

        //        WWW loadDB = new WWW ( "jar:file://" + Application.dataPath + "!/assets/" + "words.db" );  // this is the path to your StreamingAssets in android

        //        while ( !loadDB.isDone ) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check

        //        then save to Application.persistentDataPath

        //        File.WriteAllBytes ( filepath, loadDB.bytes );

        //    }



        //    open db connection

        //    string dbPath = "URI=file:" + filepath;

        //    dbConnection = new SqliteConnection ( dbPath );

        //    dbConnection.Open ();
        //}

        public void setDatabaseConnection ()
        {
            string dbPath = "URI=file:" + Application.dataPath + "/StreamingAssets/words.db";
            dbConnection = new SqliteConnection ( dbPath );
            dbConnection.Open ();
        }

        private void createUsefulObjects ()
        {
            potentials = new List<IDataReader> ();
            letters = new List<List<string>> ();
            letterIDs = new List<int> ();

            int maxLength = 0;

            foreach ( List<int> acrossWord in acrossWords )
            {
                dbcmd = dbConnection.CreateCommand ();
                dbcmd.CommandText = "SELECT word FROM w0";
                potentials.Add ( dbcmd.ExecuteReader () );
                dbcmd.Dispose ();

                if ( acrossWord.Count > maxLength )
                {
                    maxLength = acrossWord.Count;
                }
            }

            foreach ( string slot in puzzle )
            {
                letters.Add ( new List<string> () );
            }

            for ( int i = 0; i < size.x * size.y; i++ )
            {
                letterIDs.Add ( queryWords[i].IndexOf ( i ) );
            }

            lastUsefulBox = acrossWords[acrossWords.Count - 1][0];
        }

        private void createCrosswordAndCollectGarbage ( Stopwatch timer )
        {
            if ( isEmergency )
            {
                cleanAllLists ();
                generateCrossword ();
            }

            else
            {
                crossword.creationDate = DateTime.Now.ToString ();
                crossword.creationTime = timer.Elapsed.TotalSeconds.ToString ();
                crossword.difficulty = ( int ) sliders[0].value;

                crossword.x = size.x;
                crossword.y = size.y;

                crossword.polePosition = acrossWords[0][0];

                crossword.category = "UNDEFINED";

                int black=0;

                foreach ( string slot in puzzle )
                {
                    crossword.acrossID.Add ( -1 );
                    crossword.downID.Add ( -1 );
                    crossword.answers.Add ( slot );
                    crossword.filledSlots.Add ( " " );

                    if ( slot.Equals ( "1" ) )
                    {
                        crossword.grid.Add ( true );
                        black++;
                    }

                    else
                    {
                        crossword.grid.Add ( false );
                    }
                }

                crossword.blackBoxPercentage = ( black * 100 ) / crossword.grid.Count;

                foreach ( List<int> word in acrossWords )
                {
                    string w ="";

                    foreach ( int slot in word )
                    {
                        w += puzzle[slot];
                        crossword.acrossID[slot] = acrossWords.IndexOf ( word );
                    }

                    crossword.acrossWords.Add ( w );
                }

                foreach ( List<int> word in downWords )
                {
                    string w ="";

                    foreach ( int slot in word )
                    {
                        w += puzzle[slot];
                        crossword.downID[slot] = downWords.IndexOf ( word );
                    }

                    crossword.downWords.Add ( w );
                }

                foreach ( string word in crossword.acrossWords )
                {
                    string query = string.Format("SELECT definition FROM words WHERE word LIKE '{0}'", word );
                    dbcmd = dbConnection.CreateCommand ();
                    dbcmd.CommandText = query;
                    IDataReader reader = dbcmd.ExecuteReader ();

                    while ( reader.Read () )
                    {
                        crossword.questionsOfAcrossWords.Add ( reader.GetString ( 0 ) );
                    }

                    dbcmd.Dispose ();
                    reader.Close ();
                }

                foreach ( string word in crossword.downWords )
                {
                    string query = string.Format("SELECT definition FROM words WHERE word LIKE '{0}'", word );
                    dbcmd = dbConnection.CreateCommand ();
                    dbcmd.CommandText = query;
                    IDataReader reader = dbcmd.ExecuteReader ();

                    while ( reader.Read () )
                    {
                        crossword.questionsOfDownWords.Add ( reader.GetString ( 0 ) );
                    }

                    dbcmd.Dispose ();
                    reader.Close ();
                }

                foreach ( string word in crossword.acrossWords )
                {
                    string query = string.Format("SELECT imageID FROM words WHERE word LIKE '{0}'", word );
                    dbcmd = dbConnection.CreateCommand ();
                    dbcmd.CommandText = query;
                    IDataReader reader = dbcmd.ExecuteReader ();

                    while ( reader.Read () )
                    {
                        crossword.imagesOfAcrossWords.Add ( reader.GetInt32 ( 0 ) );
                    }

                    dbcmd.Dispose ();
                    reader.Close ();
                }

                foreach ( string word in crossword.downWords )
                {
                    string query = string.Format("SELECT imageID FROM words WHERE word LIKE '{0}'", word );
                    dbcmd = dbConnection.CreateCommand ();
                    dbcmd.CommandText = query;
                    IDataReader reader = dbcmd.ExecuteReader ();

                    while ( reader.Read () )
                    {
                        crossword.imagesOfDownWords.Add ( reader.GetInt32 ( 0 ) );
                    }

                    dbcmd.Dispose ();
                    reader.Close ();
                }

                cleanAllLists ();

                isComplete = true;
            }
        }

        private void determineSlotsOfAcrossWordsBasedOnPuzzleGrid ()
        {
            acrossWords = new List<List<int>> ();

            List<int> word1 = new List<int>();
            List<int> word2 = new List<int>();

            for ( int i = 0; i < size.y; i++ )
            {
                for ( int j = 0; j < size.x; j++ )
                {
                    while ( j < size.x && !puzzle[size.x * i + j].Equals ( "1" ) )
                    {
                        word1.Add ( size.x * i + j );
                        word2.Add ( size.x * i + j );

                        j++;

                        if ( j >= size.x || puzzle[size.x * i + j].Equals ( "1" ) )
                        {
                            acrossWords.Add ( word1 );
                            crossword.indexesOfAcrossWords.Add ( word2 );

                            word1 = new List<int> ();
                            word2 = new List<int> ();
                        }
                    }
                }
            }
        }

        private void determineQueryWordsForEachSlot ()
        {
            queryWords = new List<List<int>> ();

            foreach ( string slot in puzzle )
            {
                queryWords.Add ( new List<int> () );
            }

            int i = 0;

            foreach ( string slot in puzzle )
            {
                if ( !slot.Equals ( "1" ) )
                {
                    foreach ( List<int> downWord in downWords )
                    {
                        if ( downWord.IndexOf ( i ) != -1 )
                        {
                            foreach ( int value in downWord )
                            {
                                queryWords[i].Add ( value );
                            }
                        }
                    }
                }

                i++;
            }
        }

        private void determineSlotsOfDownWordsBasedOnPuzzleGrid ()
        {
            downWords = new List<List<int>> ();

            List<int> word1 = new List<int>();
            List<int> word2 = new List<int>();

            for ( int i = 0; i < size.x; i++ )
            {
                for ( int j = 0; j < size.y; j++ )
                {
                    while ( j < size.y && !puzzle[size.x * j + i].Equals ( "1" ) )
                    {
                        word1.Add ( size.x * j + i );
                        word2.Add ( size.x * j + i );

                        j++;

                        if ( j >= size.y || puzzle[size.x * j + i].Equals ( "1" ) )
                        {
                            downWords.Add ( word1 );
                            crossword.indexesOfDownWords.Add ( word2 );

                            word1 = new List<int> ();
                            word2 = new List<int> ();
                        }
                    }
                }
            }
        }

        private void cleanAllLists ()
        {
            foreach ( IDataReader position in potentials )
            {
                if ( !position.IsClosed )
                {
                    position.Close ();
                }
            }

            potentials.Clear ();

            foreach ( List<string> set in letters )
            {
                set.Clear ();
            }

            letters.Clear ();

            puzzle.Clear ();

            foreach ( List<int> word in acrossWords )
            {
                word.Clear ();
            }

            acrossWords.Clear ();

            foreach ( List<int> word in downWords )
            {
                word.Clear ();
            }

            downWords.Clear ();

            foreach ( List<int> word in queryWords )
            {
                word.Clear ();
            }

            queryWords.Clear ();

            freeSlots.Clear ();

            letterIDs.Clear ();
        }

        private void createPuzzleGrid ()
        {
            if ( GUIManager.isContinue )
            {
                popPuzzleFromDatabase ("To continue from last puzzle");
            }

            else if( GUIManager.isTimeout )
            {
                popPuzzleFromDatabase ( "Because of timeout" );
            }

            else
            {
                size = new Point ( getNormalSize().x, getNormalSize ().y );

                string data = Patterns.getPattern (size);

                isEmergency = false;

                puzzle = new List<string> ();

                foreach ( char value in data )
                {
                    puzzle.Add ( value.ToString () );
                }

                if ( sliders[0].value == 1.0f )
                {
                    difficulty = " ORDER BY rating DESC";
                }

                else if ( sliders[0].value == 3.0f )
                {
                    difficulty = " ORDER BY rating ASC";
                }

                else
                {
                    difficulty = " ORDER BY RANDOM()";
                }
            }
        }


        public void reverse(int x, int y)
        {
            string query = string.Format("SELECT data FROM emergency WHERE x={0} AND y={1};", x, y);

            setDatabaseConnection ();
            dbcmd = dbConnection.CreateCommand ();
            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader ();

            List<string> data = new List<string>();

            while ( reader.Read () )
            {
                data.Add ( reader.GetString ( 0 ) );
            }

            dbcmd.Dispose ();
            reader.Close ();

            foreach(string letter in data)
            {
                string[] temp = letter.Split('#');

                List<string> newData = new List<string>();

                for(int i=2; i<temp.Length; i++ )
                {
                    newData.Add (temp[i]);
                }

                string reverseData = "";

                for(int i=0; i<x;  i++)
                {
                    for ( int j = 0; j < y; j++ )
                    {
                        reverseData += newData[j*x + i] + "#";
                    }
                }

                reverseData = y.ToString () + "#" + x.ToString () + "#" + reverseData;

                query = string.Format ( "INSERT INTO emergency (x, y, data) VALUES ({0}, {1}, '{2}');", y, x, reverseData );

                dbcmd = dbConnection.CreateCommand ();
                dbcmd.CommandText = query;
                reader = dbcmd.ExecuteReader ();
                dbcmd.Dispose ();
                reader.Close ();
            }

            dbConnection.Close ();
            dbConnection = null;
        }

        public void popPuzzleFromDatabase (string reason)
        {
            puzzle = new List<string> ();

            string query = "";

            if ( reason.Equals ( "To continue from last puzzle" ) )
            {
                query = "SELECT data FROM history";
            }

            else if ( reason.Equals ( "Because of timeout" ) )
            {
                query = string.Format ( "SELECT data FROM emergency WHERE x={0} AND y={1} ORDER BY RANDOM() LIMIT 1", getNormalSize().x, getNormalSize ().y );
            }

            dbcmd = dbConnection.CreateCommand ();
            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader ();

            string data = "";

            while ( reader.Read () )
            {
                data = reader.GetString ( 0 );
            }

            dbcmd.Dispose ();
            reader.Close ();

            string[] words = data.Split('#');

            size = new Point ( int.Parse ( words[0] ), int.Parse ( words[1] ) );

            for ( int i = 2; i < 2 + ( size.x * size.y ); i++ )
            {
                puzzle.Add ( words[i] );
            }

            if ( reason.Equals ( "To continue from last puzzle" ) )
            {
                for ( int i = 2 + ( size.x * size.y ); i < 2 + ( 2 * ( size.x * size.y ) ); i++ )
                {
                    crossword.filledSlots.Add ( words[i] );
                }
            }

            else if ( reason.Equals ( "Because of timeout" ) )
            {
                query = string.Format ( "DELETE FROM emergency WHERE data = '{0}'", data );
                dbcmd = dbConnection.CreateCommand ();
                dbcmd.CommandText = query;
                reader = dbcmd.ExecuteReader ();

                dbcmd.Dispose ();
                reader.Close ();
            }
        }

        public void saveCheckPointToDatabase ()
        {
            string data = crossword.x.ToString() + "#" + crossword.y.ToString() + "#";

            foreach ( string letter in crossword.answers )
            {
                data += letter + "#";
            }

            foreach ( string letter in crossword.filledSlots )
            {
                data += letter + "#";
            }

            string query = string.Format("UPDATE history SET data = '{0}'", data);

            setDatabaseConnection ();
            dbcmd = dbConnection.CreateCommand ();
            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader ();
            dbcmd.Dispose ();
            reader.Close ();

            dbConnection.Close ();
            dbConnection = null;
        }

        public void getPreferences ()
        {
            string query = "SELECT data FROM preferences";

            setDatabaseConnection ();
            dbcmd = dbConnection.CreateCommand ();
            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader ();

            string data = "";

            while ( reader.Read () )
            {
                data = reader.GetString ( 0 );
            }

            string[] preferences = data.Split('#');

            lifeCounter = int.Parse ( preferences[0] );

            ButtonManager.Instance.sliders[0].value = int.Parse ( preferences[1] );
            ButtonManager.Instance.sliders[1].value = int.Parse ( preferences[2] );
            ButtonManager.Instance.sliders[2].value = int.Parse ( preferences[3] );
            ButtonManager.Instance.sliders[3].value = int.Parse ( preferences[4] );

            dbcmd.Dispose ();
            reader.Close ();
            dbConnection.Close ();
            dbConnection = null;
        }

        public void savePreferences ()
        {
            string data = (lifeCounter + 1).ToString() + "#";

            foreach ( Slider slider in ButtonManager.Instance.sliders )
            {
                data += ( ( int ) ( slider.value ) ).ToString () + "#";
            }

            string query = string.Format("UPDATE preferences SET data = '{0}'", data);

            setDatabaseConnection ();
            dbcmd = dbConnection.CreateCommand ();
            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader ();
            dbcmd.Dispose ();
            reader.Close ();
            dbConnection.Close ();
            dbConnection = null;
        }

        public void savePuzzle ()
        {
            string data = crossword.x.ToString() + "#" + crossword.y.ToString() + "#";

            foreach ( string letter in crossword.answers )
            {
                data += letter + "#";
            }

            string query = string.Format("INSERT INTO emergency (x, y, data) VALUES ({0}, {1}, '{2}');", crossword.x, crossword.y, data);

            setDatabaseConnection ();
            dbcmd = dbConnection.CreateCommand ();
            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader ();
            dbcmd.Dispose ();
            reader.Close ();
            dbConnection.Close ();
            dbConnection = null;
        }

        public Point getSize ()
        {
            int x = (int) sliders[2].value;
            int y = (int) sliders[3].value;

            x = x == 5 ? 5 : x == 6 ? 7 : x == 7 ? 10 : x == 8 ? 13 : 13;
            y = y == 5 ? 5 : y == 6 ? 7 : y == 7 ? 10 : y == 8 ? 13 : 13;

            return new Point ( x, y );
        }

        private Point getNormalSize ()
        {
            return Patterns.normalize (getSize().x, getSize().y);
        }
    }
}