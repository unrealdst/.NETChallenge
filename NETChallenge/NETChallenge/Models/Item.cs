using System;

namespace NETChallenge.Models
{
    public class Item
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public String Name { get; set; }
        public decimal PriceForUnit { get; set; }
    }
}
