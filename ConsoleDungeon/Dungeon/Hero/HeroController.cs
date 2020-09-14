using ConsoleDungeon.Dungeon.Hero.Model;
using ConsoleDungeon.Dungeon.Shared;
using ConsoleDungeon.Dungeon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDungeon.Dungeon.Hero
{
    public class HeroController
    {
        public HeroController(Point coords)
        {
            Player = new Player(coords);
        }

        public HeroController(Player player)
        {
            Player = player;
        }

        public Point GetNextCoordinate(string direction)
        {
            Point newCoordinate = null;
            switch (direction)
            {
                case Constants.NORTH:
                    newCoordinate = new Point(Player.Coordinate.X, Player.Coordinate.Y - 1);
                    break;
                case Constants.SOUTH:
                    newCoordinate = new Point(Player.Coordinate.X, Player.Coordinate.Y + 1);
                    break;
                case Constants.WEST:
                    newCoordinate = new Point(Player.Coordinate.X - 1, Player.Coordinate.Y);
                    break;
                case Constants.EAST:
                    newCoordinate = new Point(Player.Coordinate.X + 1, Player.Coordinate.Y);
                    break;
            }

            return newCoordinate;
        }

        public void Move(Point coords)
        {
            Player.Coordinate = coords;
        }

        public void RemoveHealth()
        {
            Player.Health--;
        }

        public void AddItemsToBag(List<Item> items)
        {
            Player.Bag.Items.AddRange(items);
            var message = "You find a ";
            for (int i = 0; i < items.Count; i++)
            {
                message += items[i].Type.ToString() + (i == items.Count - 1 ? " and put in your bag." : ", ");
            }
            Console.WriteLine(message);
        }

        public void ListBagItems()
        {
            if(Player.Bag.Items.Count > 0)
            {
                var message = "You has a ";
                for (int i = 0; i < Player.Bag.Items.Count; i++)
                {
                    message += Player.Bag.Items[i].Type.ToString() + (i == Player.Bag.Items.Count - 1 ? " in your bag." : ", ");
                }
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine("Your bag is empty!");
            }
        }

        public Player Player;
    }
}
