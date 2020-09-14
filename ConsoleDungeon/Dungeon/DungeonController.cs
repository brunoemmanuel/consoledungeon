using ConsoleDungeon.Dungeon.Enemy;
using ConsoleDungeon.Dungeon.Hero;
using ConsoleDungeon.Dungeon.Hero.Model;
using ConsoleDungeon.Dungeon.Map;
using ConsoleDungeon.Dungeon.Map.Model;
using ConsoleDungeon.Dungeon.Shared;
using ConsoleDungeon.Dungeon.Shared.Model;
using ConsoleDungeon.Dungeon.Shared.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDungeon.Dungeon
{
    public class DungeonController
    {
        public DungeonController()
        {
            Console.WriteLine("Welcome to Console Dungeon\n");
            Console.WriteLine("Type \"help\" to see a list os commands.");
            ListenCommand();
        }

        private void ListenCommand()
        {
            var command = Console.ReadLine();
            if(!HeroIsDead() && !EnemyIsDead())
            {
                switch (command)
                {
                    case Constants.HELP:
                        PrintHelpCommands();
                        break;
                    case Constants.NEW:
                        CreateNewGame();
                        break;
                    case Constants.SAVE:
                        SaveGame();
                        break;
                    case Constants.LOAD:
                        LoadGame();
                        break;
                    case Constants.NORTH:
                    case Constants.SOUTH:
                    case Constants.WEST:
                    case Constants.EAST:
                        MovePlayer(command);
                        break;
                    case Constants.SEARCH:
                        SearchItems();
                        break;
                    case Constants.BAG:
                        ListBagItems();
                        break;
                    case Constants.GOD:
                        EnableGodMode();
                        break;
                    default:
                        Console.WriteLine("Command not recognized. Try \"help\".");
                        break;
                }
            }
            else
            {
                if(HeroIsDead())
                    Console.WriteLine("You dead!");

                if(EnemyIsDead())
                    Console.WriteLine("Enemy is dead! You Win!");
            }
            
            ListenCommand();
        }

        private void EnableGodMode()
        {
            if (HeroController == null || EnemyController == null || MapController == null)
                return;

            MapController.GodMode = !MapController.GodMode;
            MapController.Print(HeroController.Player.Coordinate, EnemyController.Character.Coordinate);
        }

        private void CreateNewGame()
        {
            MapController = new MapController();
            HeroController = new HeroController(MapController.GetRandomRoomCoordinate());
            EnemyController = new EnemyController(MapController.GetInitialEnemyCoordinate(HeroController.Player.Coordinate));
            Console.WriteLine("New dungeon was created.");
            MapController.Print(HeroController.Player.Coordinate, EnemyController.Character.Coordinate);
        }

        private void SaveGame()
        {
            if (HeroController == null || EnemyController == null || MapController == null)
                return;

            PersistenceController.Instance.Save(MapController.GetRooms());
            PersistenceController.Instance.Save(HeroController.Player);
            PersistenceController.Instance.Save(EnemyController.Character);
            Console.WriteLine("Game saved successfully.");
        }

        private void LoadGame()
        {
            var rooms = PersistenceController.Instance.Load<List<Room>>();
            var player = PersistenceController.Instance.Load<Player>();
            if(rooms != null && player != null)
            {
                MapController = new MapController(rooms);
                HeroController = new HeroController(player);
                Console.WriteLine("Save loaded successfully.");
                MapController.Print(HeroController.Player.Coordinate, EnemyController.Character.Coordinate);
            }
            else
            {
                Console.WriteLine("There is no saved data. Try a new game");
            }
        }

        private void MovePlayer(string command)
        {
            if (HeroController == null || EnemyController == null || MapController == null)
                return;
            
            Point newCoordinate = HeroController.GetNextCoordinate(command);
            
            if(MapController.CanMove(newCoordinate))
            {
                if (MapController.SameRoom(HeroController.Player.Coordinate, EnemyController.Character.Coordinate))
                {
                    HeroController.RemoveHealth();
                    Console.WriteLine("You lost 1 health. Now you has " + HeroController.Player.Health + " left.");
                }

                HeroController.Move(newCoordinate);
                if (EnemyController.CanMove())
                {
                    var nextEnemyMove = MapController.GetNextEnemyCoordinate(newCoordinate, EnemyController.Character.Coordinate);
                    EnemyController.Move(nextEnemyMove);
                }
                
                MapController.Print(HeroController.Player.Coordinate, EnemyController.Character.Coordinate);

                if(MapController.NextRoom(HeroController.Player.Coordinate, EnemyController.Character.Coordinate))
                {
                    Console.WriteLine("The enemy is close to you!");
                }
                else if(MapController.SameRoom(HeroController.Player.Coordinate, EnemyController.Character.Coordinate))
                {
                    if(HeroController.Player.Bag.Items.Count > 0 && HeroController.Player.Bag.Items[0].Type == Item.ItemType.Sword)
                    {
                        EnemyController.Dead = true;
                        Console.WriteLine("You killed the enemy with the Sword. You are free to go.");
                    }
                    else
                    {
                        Console.WriteLine("The enemy is in front of you. You have no Sword, RUN!.");
                    }
                }
            }
            else
            {
                Console.WriteLine("There is a wall here!");
            }
        }

        private void SearchItems()
        {
            if (HeroController == null || MapController == null)
                return;

            var items = MapController.GetRoomItems(HeroController.Player.Coordinate);
            if(items.Count > 0)
            {
                HeroController.AddItemsToBag(items);
                MapController.RemoveRoomItems(HeroController.Player.Coordinate);
            }
            else
            {
                Console.WriteLine("There are nothing here.");
            }
        }

        private void ListBagItems()
        {
            if (HeroController == null)
                return;

            HeroController.ListBagItems();
        }

        private bool HeroIsDead()
        {
            return HeroController != null && HeroController.Player.Health == 0;
        }

        private bool EnemyIsDead()
        {
            return EnemyController != null && EnemyController.Dead;
        }

        private void PrintHelpCommands()
        {
            Console.WriteLine("\"new\" Create a new game.");
            Console.WriteLine("\"load\" Load a saved game.");

            if(MapController != null)
            {
                Console.WriteLine("\"save\" Save the current game.");
                Console.WriteLine("\"north, south, west, east\" Move between rooms.");
                Console.WriteLine("\"search\" Look around and search for items.");
            }

            if (HeroController != null)
            {
                Console.WriteLine("\"bag\" See your items");
            }
        }

        private MapController MapController;

        private HeroController HeroController;

        private EnemyController EnemyController;
    }
}
