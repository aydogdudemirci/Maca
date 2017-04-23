using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace Maca
{
    public class BackgroundGenerator : Singleton<BackgroundGenerator>
    {
        public GameObject gameScreen;
        public bool onProgress;
        Stopwatch gametime;
        List<Point> sizes;
        bool crosswordStarving;

        Thread GO;

        public static Point criticalSize;

        int stage;

        private void Awake ()
        {
            instance = this;
            onProgress = false;
        }

        private void Start ()
        {
            sizes = new List<Point> ();

            sizes.Add ( new Point ( 5, 5 ) );
            sizes.Add ( new Point ( 5, 7 ) );
            sizes.Add ( new Point ( 5, 10 ) );
            sizes.Add ( new Point ( 5, 13 ) );
            sizes.Add ( new Point ( 7, 7 ) );
            sizes.Add ( new Point ( 7, 10 ) );
            sizes.Add ( new Point ( 7, 13 ) );
            sizes.Add ( new Point ( 10, 10 ) );
            sizes.Add ( new Point ( 10, 13 ) );
            sizes.Add ( new Point ( 13, 13 ) );
        }

        public void reset ()
        {
            crosswordStarving = false;
            onProgress = false;

            gametime = new Stopwatch ();
            gametime.Start ();

            stage = 0;
        }

        public void processDecision ()
        {
            if ( gameScreen.activeSelf )
            {
                if ( stage.Equals ( 0 ) && !onProgress )
                {
                    StartCoroutine ( investigateStarving () );
                }

                else if ( stage.Equals ( 1 ) && crosswordStarving && gametime.Elapsed.TotalSeconds > 10.0f && !onProgress )
                {
                    StartCoroutine ( appeaseCrosswordStarving () );
                }
            }
        }

        private IEnumerator appeaseCrosswordStarving ()
        {
            onProgress = true;

            yield return new WaitForSeconds ( 5.0f );

            Generator.Instance.setDatabaseConnection ();

            GO = new Thread ( Generator.Instance.generateCrossword );
            GO.IsBackground = true;

            GUIManager.totalTime.Reset ();
            GUIManager.totalTime.Start ();

            Generator.Instance.isComplete = false;
            Generator.Instance.isBackground = true;

            GO.Start ();

            yield return new WaitUntil ( ()=> Generator.Instance.isComplete );

            Generator.Instance.isComplete = false;

            if ( GUIManager.isTimeout)
            {
                GUIManager.isTimeout = false;
                UnityEngine.Debug.Log ( "üretilemedi" );
            }

            else
            {
                Generator.Instance.savePuzzle ();
                UnityEngine.Debug.Log ( "üretildi" );
            }

            Generator.Instance.isBackground = false;
            GUIManager.totalTime.Reset ();

            stage++;
            onProgress = false;
        }

        private IEnumerator investigateStarving ()
        {
            onProgress = true;

            yield return new WaitForEndOfFrame ();

            IDbConnection dbConnection;
            IDbCommand dbcmd;
            IDataReader reader;

            yield return new WaitForEndOfFrame ();

            string dbPath = "URI=file:" + Application.dataPath + "/StreamingAssets/words.db";
            dbConnection = new SqliteConnection ( dbPath );
            dbConnection.Open ();

            yield return new WaitForEndOfFrame ();

            foreach ( Point size in sizes )
            {
                string query = string.Format("SELECT COUNT(data) FROM emergency WHERE x={0} AND y={1}", size.x, size.y);

                dbcmd = dbConnection.CreateCommand ();
                dbcmd.CommandText = query;
                reader = dbcmd.ExecuteReader ();
                dbcmd.Dispose ();

                while ( reader.Read () )
                {
                    if ( reader.GetInt32 ( 0 ) < 15 )
                    {
                        reader.Dispose ();

                        criticalSize = new Point ( size.x, size.y );

                        crosswordStarving = true;

                        break;
                    }
                }

                if ( crosswordStarving )
                {
                    break;
                }
            }

            dbConnection.Close ();
            stage++;
            onProgress = false;
        }
    }
}
