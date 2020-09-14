using ConsoleDungeon.Dungeon.Shared;
using ConsoleDungeon.Dungeon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDungeon.Dungeon.Enemy
{
    public class EnemyController
    {
        private const int MOVES = 2;

        public EnemyController(Point coords)
        {
            Character = new Character(coords);
            Init();
        }

        public EnemyController(Character enemy)
        {
            Character = enemy;
            Init();
        }

        private void Init()
        {
            CountMoves = 0;
            Dead = false;
        }

        public bool CanMove()
        {
            CountMoves++;
            if(CountMoves == MOVES)
            {
                CountMoves = 0;
                return true;
            }

            return false;
        }

        public void Move(Point coords)
        {
            Character.Coordinate = coords;
        }

        public Character Character;
        public bool Dead;

        private int CountMoves;
    }
}
