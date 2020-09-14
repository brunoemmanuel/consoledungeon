using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDungeon.Dungeon.Shared.Model
{
    [Serializable()]
    public class Point
    {
        public Point() { }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return X + "," + Y;
        }

        public int X;

        public int Y;
    }
}
