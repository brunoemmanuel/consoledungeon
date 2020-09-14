using ConsoleDungeon.Dungeon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDungeon.Dungeon.Hero.Model
{
    [Serializable()]
    public class Player : Character
    {
        private const int HEALTH = 3;

        public Player(Point coords) : base(coords)
        {
            Health = HEALTH;
        }

        public Bag Bag = new Bag();

        public int Health;
    }
}
