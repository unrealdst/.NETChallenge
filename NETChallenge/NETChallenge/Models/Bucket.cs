using System.Collections.Generic;

namespace NETChallenge.Models
{
    public class Bucket
    {
        public Bucket()
        {
            if(_items == null)
            {
                _items = new List<Item>();
            }
        }

        private static List<Item> _items;

        public List<Item> Items { get => _items; set => _items = value; }
    }
}
