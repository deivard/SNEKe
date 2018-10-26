using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNEKe
{
#pragma warning disable 660,661 // Don't want to implement equals..
    public class Position
#pragma warning restore 660,661
    {
        private int y;
        private int x;

        public Position(int x, int y)
        {
            X = x; Y = y;
        }

        public static bool operator ==(Position pos1, Position pos2)
        {
            return pos1.X == pos2.X && pos1.Y == pos2.Y;
        }

        public static bool operator !=(Position pos1, Position pos2)
        {
            return pos1.X != pos2.X && pos1.Y != pos2.Y;
        }
        

        public int X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }
    }
}
