using System;

namespace BucketRepository.Models
{
    public class ItemDataModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public String Name { get; set; }
        public decimal PriceForUnit { get; set; }
    }
}
