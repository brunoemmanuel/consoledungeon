using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDungeon.Dungeon.Shared.Model
{
    [Serializable()]
    public class Item
    {
        public enum ItemType
        {
            Sword,
            Key
        }

        public Item(ItemType itemType)
        {
            Type = itemType;
        }

        public ItemType Type { get; }
    }
}
