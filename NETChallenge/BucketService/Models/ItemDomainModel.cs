using System;

namespace BucketService.Models
{
    public class ItemDomainModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public String Name { get; set; }
        public decimal PriceForUnit { get; set; }
    }
}
