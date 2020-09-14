using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDungeon.Dungeon.Shared.Model
{
    [Serializable()]
    public class Character
    {
        public Character(Point coords)
        {
            Coordinate = coords;
        }

        public Point Coordinate;
    }
}
