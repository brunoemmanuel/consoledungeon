using ConsoleDungeon.Dungeon.Hero.Model;
using ConsoleDungeon.Dungeon.Map.Model;
using ConsoleDungeon.Dungeon.Shared.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ConsoleDungeon.Dungeon.Shared.Persistence
{
    public class PersistenceController
    {
        private static PersistenceController instance;

        public PersistenceController() { }

        public void Save(object data)
        {
            var fileName = GetFileName(data.GetType());
            try
            {
                using (Stream stream = File.Open(fileName, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, data);
                }
            }
            catch (IOException e)
            {
                //Console.WriteLine(e.Message);
            }
        }

        public T Load<T>()
        {
            var fileName = GetFileName(typeof(T));
            T result = default(T);
            try
            {
                using (Stream stream = File.Open(fileName, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();

                    result = (T)bin.Deserialize(stream);
                }
            }
            catch (IOException e)
            {
                //Console.WriteLine(e.Message);
            }

            return result;
        }

        private string GetFileName(Type type)
        {
            if(typeof(List<Room>).Equals(type))
            {
                return "map.bin";
            }
            else if(typeof(Player).Equals(type))
            {
                return "player.bin";
            }
            else if (typeof(Character).Equals(type))
            {
                return "enemy.bin";
            }
            else
            {
                return "";
            }
        }

        public static PersistenceController Instance {
            get {
                if(instance == null)
                {
                    instance = new PersistenceController();
                }

                return instance;
            }
        }
    }
}
