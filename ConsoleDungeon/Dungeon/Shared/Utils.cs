using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDungeon.Dungeon.Shared
{
    public class Utils
    {
        private static Random rand = new Random();

        public static int RandomNumberGenerator(int start, int end)
        {
            int num = rand.Next(start, end);
            return num;
        }
    }
}
