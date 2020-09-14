using ConsoleDungeon.Dungeon.Map.Model;
using ConsoleDungeon.Dungeon.Shared;
using ConsoleDungeon.Dungeon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDungeon.Dungeon.Map
{
    public class MapFactory
    {
        public MapFactory()
        {
            Rooms = new List<Room>();
        }

        public List<Room> Build()
        {
            CreateRoom(0, 0);

            return Rooms;
        }

        public static bool HasRoom(Point coods, List<Room> rooms)
        {
            foreach (Room room in rooms)
            {
                if (room.Coordinate.X == coods.X && room.Coordinate.Y == coods.Y)
                {
                    return true;
                }
            }

            return false;
        }

        private void CreateRoom(int x, int y)
        {
            if (Rooms.Count == 15)
                return;

            var room = new Room(x, y);
            Rooms.Add(room);

            var nextRooms = GetNextRooms(room.Coordinate);

            for (int i = 0; i < nextRooms.Count; i++)
            {
                CreateRoom(nextRooms[i].X, nextRooms[i].Y);
            }
        }

        private List<Point> GetNextRooms(Point coord)
        {
            var tempRooms = new List<Point> { new Point(coord.X, coord.Y + 1), new Point(coord.X, coord.Y - 1), new Point(coord.X + 1, coord.Y), new Point(coord.X - 1, coord.Y) };
            var nextRooms = new List<Point>();
            for (int i = 0; i < tempRooms.Count; i++)
            {
                if (!HasRoom(tempRooms[i], Rooms))
                {
                    nextRooms.Add(tempRooms[i]);
                }
            }

            var choosedRooms = new List<Point>();
            var numberOfDoors = NumberOfDoors(nextRooms.Count);

            for (int i = 0; i < numberOfDoors; i++)
            {
                var index = Utils.RandomNumberGenerator(0, nextRooms.Count - 1);
                var coordinate = nextRooms[index];
                choosedRooms.Add(coordinate);
                nextRooms.Remove(coordinate);
            }
            return choosedRooms;
        }

        private static int NumberOfDoors(int max)
        {
            var value = Utils.RandomNumberGenerator(1, 100);
            if (value >= 1 && value < 70)
            {
                return 1;
            }
            else if (value >= 70 && value < 90 && max <= 2)
            {
                return 2 > max ? max : 2;
            }
            else if (value >= 90 && value < 95 && max <= 3)
            {
                return 3 > max ? max : 3;
            }
            else
            {
                return 4 > max ? max : 4;
            }
        }

        private List<Room> Rooms;
    }
}
