using System;
using System.Collections.Generic;
using System.Linq;

namespace Maca
{
    public class Patterns : Singleton<Patterns>
    {
        static List<List<List<string>>> gridPatterns;
    
        void Awake ()
        {
            instance = this;
        }

        private void Start ()
        {
            createPatterns ();
        }

        private void createPatterns ()
        {
            gridPatterns = new List<List<List<string>>> ();

            for ( int i = 0; i < 16; i++ )
            {
                gridPatterns.Add ( new List<List<string>> () );

                for ( int j = 0; j < 16; j++ )
                {
                    gridPatterns[i].Add ( new List<string> () );
                }
            }

            gridPatterns[5][5].Add ( new string ( ' ', 25 ) );

            gridPatterns[5][7].Add ( "                1 1                " );
            gridPatterns[5][7].Add ( "           1           1           " );
            gridPatterns[5][7].Add ( "             1       1             " );
            gridPatterns[5][7].Add ( "      1          1          1      " );
            gridPatterns[5][7].Add ( "        1        1        1        " );
            gridPatterns[5][7].Add ( "                  1   1            " );
            gridPatterns[5][7].Add ( "            1   1                  " );
            gridPatterns[5][7].Add ( "             1       1     1       " );
            gridPatterns[5][7].Add ( "       1     1            1        " );
            gridPatterns[5][7].Add ( "       1        1           1      " );

            gridPatterns[5][10].Add ( "           1  1        1 1           1            " );
            gridPatterns[5][10].Add ( "         1      1     1     1      1              " );
            gridPatterns[5][10].Add ( "           1 1        1       1   1               " );
            gridPatterns[5][10].Add ( "              1 1   1       1   1                 " );
            gridPatterns[5][10].Add ( "           1      1   1      1      1             " );
            gridPatterns[5][10].Add ( "       1             1     1     1        1       " );
            gridPatterns[5][10].Add ( "       1       1          1       1   1           " );
            gridPatterns[5][10].Add ( "             1       1  1       1  1              " );
            gridPatterns[5][10].Add ( "          1        1 1          1          1      " );
            gridPatterns[5][10].Add ( "       1        1      1          1  1            " );

            gridPatterns[5][13].Add ( "         1 1     1  1        1   1  1          1 11       1      " );
            gridPatterns[5][13].Add ( "      1           1 1           1           1 1      1           " );
            gridPatterns[5][13].Add ( "                  1  1        1 1 1        1  1                  " );
            gridPatterns[5][13].Add ( "                1 1      1      1      1      1 1                " );
            gridPatterns[5][13].Add ( "            1   1      1      1        1       1   1             " );
            gridPatterns[5][13].Add ( "    1  1         1  1   1           1 1      1   1  1       1    " );
            gridPatterns[5][13].Add ( "                    1 1 1      1 1             1  1   1          " );
            gridPatterns[5][13].Add ( "       1      1 1      1  1       1  1      1 1          1      1" );
            gridPatterns[5][13].Add ( "       1  1   1      1 1        1        1 1      1   1  1       " );
            gridPatterns[5][13].Add ( "       1  1   1      1 1                 1 1      1   1  1       " );

            gridPatterns[7][7].Add ( "   1            1    1  1  1    1            1   " );
            gridPatterns[7][7].Add ( "    1   1           1   1   1           1   1    " );
            gridPatterns[7][7].Add ( "        1 1 1         1 1 1         1 1 1        " );
            gridPatterns[7][7].Add ( "        1 1 1          1 1          1 1 1        " );
            gridPatterns[7][7].Add ( "   1        1        1  1  1        1        1   " );
            gridPatterns[7][7].Add ( "   1    1            1  1  1            1    1   " );
            gridPatterns[7][7].Add ( "        1   1   1 1     1     1 1   1   1        " );
            gridPatterns[7][7].Add ( "   1    1   1   1 1           1 1   1   1    1   " );
            gridPatterns[7][7].Add ( "   1            1 1  1     1  1 1            1   " );
            gridPatterns[7][7].Add ( "   1           1   1    1   1     1  1 1         " );
            gridPatterns[7][7].Add ( "     1    1    1           1    1    1    1      " );
            gridPatterns[7][7].Add ( "      1    1    1    1           1    1    1     " );

            gridPatterns[7][10].Add ( "     1   1        1  1     1    1   1        1        1  1        1   " );
            gridPatterns[7][10].Add ( "   1        1 1        1        1        1 1        1   1    1        " );
            gridPatterns[7][10].Add ( "     1    1    1           1    1    1    1    1    1    1         1  " );
            gridPatterns[7][10].Add ( "   1    1   1           1   1     1  1 1   1   1    1   1     1   1   " );
            gridPatterns[7][10].Add ( "          1    1   1    1   1     1  1 1          1   1    1   1     1" );
            gridPatterns[7][10].Add ( "           1   1 1        1 1 1        1 1  1       1    1   1    1   " );
            gridPatterns[7][10].Add ( "        1   1    1   1  1  1         1 1  1     1   1    1   1    1   " );
            gridPatterns[7][10].Add ( "    1 1  1       1    1   1    1   1     1  1     1  1       1   1    " );
            gridPatterns[7][10].Add ( "          1    1         1  1 1   1     1  1 1        1   1        1  " );
            gridPatterns[7][10].Add ( "    1    1     1   1    1     1    1   1 1  1     1   1     1    1    " );
            gridPatterns[7][10].Add ( "      1   1    1   1    1     1    1   1 1  1     1   1    1       1  " );
            gridPatterns[7][10].Add ( "   1    1   1    1       1   1    1   1   1    1    1       1   1     " );
            gridPatterns[7][10].Add ( "   1    1   1     1    1       1       1  1     1  1     1   1    1   " );

            gridPatterns[7][13].Add ( "     1    1    1           1    1    1    1    1    1    1    1    1    1    1    1    1   " );
            gridPatterns[7][13].Add ( "   1      1    1   1     1  1 1   1     1    1    1     1   1 1  1     1   1    1      1   " );
            gridPatterns[7][13].Add ( "     1  1         1    1    1  1  1    1    1     1   1    1   1   1 1  1     1   1    1   " );
            gridPatterns[7][13].Add ( "   1    1          1    1   1     1  1 1     1     1 1  1     1   1    1          1    1   " );
            gridPatterns[7][13].Add ( "    1     1    1          1    1   1     1  1 1  1     1   1    1          1    1     1    " );
            gridPatterns[7][13].Add ( "   1    1   1    1         11 1 1       1    1    1       1 1 11         1    1   1    1   " );
            gridPatterns[7][13].Add ( "    1    1         1 1  1       1 1 1       1 1       1 1 1       1  1 1         1    1    " );
            gridPatterns[7][13].Add ( "   1    1   1    1     1    1     1    1   1   1   11   1     1    1     1    1   1    1   " );
            gridPatterns[7][13].Add ( "        1   1      1   1    1   1     1  1 1        1 111        1 1          1   1    1   " );
            gridPatterns[7][13].Add ( "   1    1       1   1   1   1   1       1  1 1        1 1   1     1     1   1 1        1   " );
            gridPatterns[7][13].Add ( "          1     1 1   1   1    1   1   1    1   1   1     1 1  1     1   1 1  1         1  " );

            gridPatterns[10][10].Add ( "       1       1       1       1      1       1       1    1  1    1  1    1       1    1  1    1   " );
            gridPatterns[10][10].Add ( "       1       1       1    1  1    1       1       1      11      1       1       1    1  1    1   " );
            gridPatterns[10][10].Add ( "      1       1       1      11      1       1       1    1  1    1       1    1  1    1  1    1   1" );
            gridPatterns[10][10].Add ( "     1       1     1  1    1  1    1       1    1  1    1       1    1  1    1  1    1       1    1 " );
            gridPatterns[10][10].Add ( "    1    1 1    1      1    1   1   1   1   1   1  1   1 1      1    1  1   1   1   1   1   1   1   " );
            gridPatterns[10][10].Add ( "     1       1    1  1    1       1    1  1    1  1    1       1    1  1    1       1    1  1    1  " );
            gridPatterns[10][10].Add ( "     1      1    1      1    1 1    1      1    1 1    1      1    1      1    1 1    1      1    1 " );
            gridPatterns[10][10].Add ( "      1       1   1   1   1        1    1      1 1   1       1    1       1    1 1     1  1    1    " );
            gridPatterns[10][10].Add ( "      1       1   1   1            1    1      1 1   1       1    1       1    1 1     1       1    " );
            gridPatterns[10][10].Add ( "    1   1  1    1      1     11    1 1     1       1    1 1     1     1 1    1 1     1       1   1  " );
            gridPatterns[10][10].Add ( "     1     1      1    1  1   1    1      1    1      1    1   1       1   1  1       1       1     " );
            gridPatterns[10][10].Add ( "      1    1  1   1       1     1  1    1      1 1   1 1     1    1     1 1    1   1   1       1    " );

            gridPatterns[10][13].Add ( "      1       1    1  1    1  1    1       1    1  1    1       1    1  1    1  1    1       1    1  1    1       1   1   1   1   " );
            gridPatterns[10][13].Add ( "    1      1    1      1    1 1    1      1    1      1    1 1    1      1    1 1    1      1    1      1    1 1    1      1    1 " );
            gridPatterns[10][13].Add ( "    1    1  1    1  1    1       1    1  1    1       1    1  1    1  1    1       1    1  1    1       1    1  1    1  1    1    " );
            gridPatterns[10][13].Add ( "             1    1  1    1       1    1  1    1  1    1       1    1  1    1       1    1  1    1       1       1    1  1    1   " );
            gridPatterns[10][13].Add ( "       1       1       1    1  1    1       1    1  1    1  1    1       1    1  1    1       1    1  1    1  1    1       1      " );
            gridPatterns[10][13].Add ( "           1    1      1    1 1    1      1    1      1    1 1    1      1    1 1    1      1    1      1    1 1    1      1    1 " );
            gridPatterns[10][13].Add ( "           1    1      1    1  1   1           1  1   1    1  1    1      1      1    1      1    1 1    1      1    1      1     " );

            gridPatterns[13][13].Add ( "      1    1     1    1     1    1    11    1    1     1    1     1    1    1     1    1     1    1    11    1    1     1    1     1    1    1     1    1     1    1     " );
        }

        public static Point normalize ( int x, int y )
        {
            return gridPatterns[x][y].Count > 0 ? new Point ( x, y ) : new Point ( y, x );
        }

        public static string getPattern(Point size)
        {
            System.Random randomize = new System.Random();

            int random = randomize.Next ( gridPatterns[size.x][size.y].Count);
            
            return gridPatterns[size.x][size.y][random];
        }

        internal static bool reversingNecessary ( )
        {
            int x = Generator.Instance.getSize().x;
            int y = Generator.Instance.getSize().y;

            return gridPatterns[x][y].Count > 0 ? false : true;
        }
    }
}
