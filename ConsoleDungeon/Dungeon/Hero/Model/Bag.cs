using ConsoleDungeon.Dungeon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDungeon.Dungeon.Hero.Model
{
    [Serializable()]
    public class Bag
    {
        public List<Item> Items = new List<Item>();
    }
}
