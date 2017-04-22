using System;

namespace Maca
{
    public class Point
    {
        public int x;
        public int y;
        public int index;
        public bool isAcross;

        public Point ( int x, int y )
        {
            this.x = x;
            this.y = y;
        }

        public Point ( int index, bool isAcross )
        {
            this.index = index;
            this.isAcross = isAcross;
        }

        internal int getLastOfThis ( Point p )
        {
            throw new NotImplementedException ();
        }
    }
}

