using ConsoleDungeon.Dungeon.Map.Model;
using ConsoleDungeon.Dungeon.Shared;
using ConsoleDungeon.Dungeon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDungeon.Dungeon.Map
{
    public class MapController
    {
        private const int DISTANCE = 2;

        public MapController(List<Room> rooms)
        {
            SetMapRooms(rooms);
            SetDungeonSize();
        }

        public MapController()
        {
            var rooms = new MapFactory().Build();
            SetMapRooms(rooms);
            SetDungeonSize();
            AddItems();
        }

        public Point GetRandomRoomCoordinate()
        {
            var rndIndex = Utils.RandomNumberGenerator(0, Rooms.Values.Count - 1);

            var index = 0;
            Point coords = null;
            foreach (string key in Rooms.Keys)
            {
                if (index == rndIndex)
                {
                    coords = Rooms[key].Coordinate;
                }

                index++;
            }

            return coords;
        }

        public Point GetInitialEnemyCoordinate(Point heroCoords)
        {
            var x = heroCoords.X;
            var y = heroCoords.Y;
            var availableCoords = new List<Point>();

            for (int i = x - DISTANCE; i < x + DISTANCE; i++)
            {
                var point = new Point(i, y + DISTANCE);
                if (RoomExist(point))
                {
                    availableCoords.Add(point);
                }

                point = new Point(i, y - DISTANCE);
                if (RoomExist(point))
                {
                    availableCoords.Add(point);
                }

            }

            for (int i = y - DISTANCE; i < y + DISTANCE; i++)
            {
                var point = new Point(x + DISTANCE, i);
                if (RoomExist(point))
                {
                    availableCoords.Add(point);
                }

                point = new Point(x - DISTANCE, i);
                if (RoomExist(point))
                {
                    availableCoords.Add(point);
                }
            }

            var rndIndex = Utils.RandomNumberGenerator(0, availableCoords.Count - 1);

            return availableCoords[rndIndex];
        }

        public Point GetNextEnemyCoordinate(Point heroCoords, Point enemyCoords)
        {
            var availableRooms = new List<Point>();
            var northCoord = new Point(enemyCoords.X, enemyCoords.Y + 1);
            if (RoomExist(northCoord))
                availableRooms.Add(northCoord);

            var westCoord = new Point(enemyCoords.X + 1, enemyCoords.Y);
            if (RoomExist(westCoord))
                availableRooms.Add(westCoord);

            var southCoord = new Point(enemyCoords.X, enemyCoords.Y - 1);
            if (RoomExist(southCoord))
                availableRooms.Add(southCoord);

            var eastCoord = new Point(enemyCoords.X - 1, enemyCoords.Y);
            if (RoomExist(eastCoord))
                availableRooms.Add(eastCoord);

            Point tempCoord = null;
            double tempDistance = int.MaxValue;
            foreach(Point coord in availableRooms)
            {
                double xPow = Math.Pow(coord.X - heroCoords.X, 2);
                double yPow = Math.Pow(coord.Y - heroCoords.Y, 2);

                double distance = Math.Sqrt(xPow + yPow);
                if (distance < tempDistance)
                {
                    tempDistance = distance;
                    tempCoord = coord;
                } 
            }

            return tempCoord;
        }

        public bool NextRoom(Point room, Point nextRoom)
        {
            var availableRooms = new List<Point>();
            var northCoord = new Point(room.X, room.Y + 1);
            if (RoomExist(northCoord))
                availableRooms.Add(northCoord);

            var westCoord = new Point(room.X + 1, room.Y);
            if (RoomExist(westCoord))
                availableRooms.Add(westCoord);

            var southCoord = new Point(room.X, room.Y - 1);
            if (RoomExist(southCoord))
                availableRooms.Add(southCoord);

            var eastCoord = new Point(room.X - 1, room.Y);
            if (RoomExist(eastCoord))
                availableRooms.Add(eastCoord);
            
            foreach (Point coord in availableRooms)
            {
                if(coord.X == nextRoom.X && coord.Y == nextRoom.Y)
                {
                    return true;
                }
            }

            return false;
        }

        public bool SameRoom(Point room1, Point room2)
        {
            return room1.X == room2.X && room1.Y == room2.Y;
        }

        public List<Item> GetRoomItems(Point coords)
        {
            if (!Rooms.ContainsKey(coords.ToString()))
                return null;
            
            return Rooms[coords.ToString()].Items;
        }

        public bool CanMove(Point coords)
        {
            return RoomExist(coords);
        }

        public void RemoveRoomItems(Point coords)
        {
            if (!Rooms.ContainsKey(coords.ToString()))
                return;

            Rooms[coords.ToString()].Items.Clear();
        }

        public void Print(Point heroCoords, Point enemyCoords)
        {
            Console.WriteLine("");
            for (int i = Min.Y; i <= Max.Y; i++)
            {
                for(int a = 0; a < 2; a++)
                {
                    string line = "    ";
                    for (int j = Min.X; j <= Max.X; j++)
                    {
                        for (int b = 0; b < 2; b++)
                        {
                            var coord = new Point(j, i);
                            var roomChar = "0";
                            
                            if(GodMode)
                            {
                                if (heroCoords.X == j && enemyCoords.X == j && heroCoords.Y == i && enemyCoords.Y == i)
                                {
                                    roomChar = "#"; //monster + hero
                                }
                                else if (heroCoords.X == j && heroCoords.Y == i)
                                {
                                    roomChar = "*"; //hero
                                }
                                else if (enemyCoords.X == j && enemyCoords.Y == i)
                                {
                                    roomChar = "&"; //enemy
                                }
                                else if (RoomExist(coord) && Rooms[coord.ToString()].Items.Count > 0)
                                {
                                    roomChar = "$"; //items
                                }
                            }
                            else
                            {
                                if (heroCoords.X == j && heroCoords.Y == i)
                                {
                                    roomChar = "*";
                                }
                            }
                            
                            line += RoomExist(coord) ? roomChar : " ";
                        }
                    }
                    Console.WriteLine(line);
                }
            }
            Console.WriteLine("");
        }

        public List<Room> GetRooms()
        {
            var rooms = new List<Room>();
            foreach(Room room in Rooms.Values)
            {
                rooms.Add(room);
            }

            return rooms;
        }

        public List<Point> GetCoordinates()
        {
            var coordinates = new List<Point>();
            foreach (Room room in Rooms.Values)
            {
                coordinates.Add(room.Coordinate);
            }

            return coordinates;
        }

        private bool RoomExist(Point coords)
        {
            return Rooms.ContainsKey(coords.ToString());
        }

        private void SetMapRooms(List<Room> rooms)
        {
            if (Rooms == null)
                Rooms = new Dictionary<string, Room>();

            foreach (Room room in rooms)
            {
                Rooms.Add(room.Coordinate.ToString(), room);
            }
        }
        
        private void SetDungeonSize()
        {
            foreach(Room room in Rooms.Values)
            {
                int x = room.Coordinate.X;
                int y = room.Coordinate.Y;

                if (x > Max.X)
                {
                    Max.X = x;
                }

                if (x < Min.X)
                {
                    Min.X = x;
                }

                if (y > Max.Y)
                {
                    Max.Y = y;
                }

                if (y < Min.Y)
                {
                    Min.Y = y;
                }
            }
        }

        private void AddItems()
        {
            var swordIndex = Utils.RandomNumberGenerator(0, Rooms.Values.Count - 1);

            int index = 0;
            foreach(string key in Rooms.Keys)
            {
                if(index == swordIndex)
                {
                    Rooms[key].Items.Add(new Item(Item.ItemType.Sword));
                }

                index++;
            }
        }

        private Dictionary<string, Room> Rooms;
        private Point Max = new Point();
        private Point Min = new Point();

        public bool GodMode = false;
    }
}
