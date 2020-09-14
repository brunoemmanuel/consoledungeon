using ConsoleDungeon.Dungeon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDungeon.Dungeon.Map.Model
{
    [Serializable()]
    public class Room
    {
        public Room(int x, int y)
        {
            Coordinate = new Point(x, y);
        }

        public Point Coordinate { get; }

        public List<Item> Items = new List<Item>();
    }
}
