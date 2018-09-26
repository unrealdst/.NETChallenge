using BucketRepository.Models;
using System.Collections.Generic;

namespace BucketRepository
{
    public interface IBucketRepository
    {
        BucketDataModel Create();
        IEnumerable<BucketDataModel> Read();
        BucketDataModel Read(int bucketId);
        BucketDataModel Update(int bucketId, BucketDataModel bucketDataModel);
        bool Delete(int bucketId);
    }
}
