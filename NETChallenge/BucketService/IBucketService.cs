using BucketService.Models;

namespace BucketService
{
    public interface IBucketService
    {
        void Quantity(int id, int quantity);
        void Add(ItemDomainModel domainModelItem);
        BucketDomainModel Get();
    }
}
