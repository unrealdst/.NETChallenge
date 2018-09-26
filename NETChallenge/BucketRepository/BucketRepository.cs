using System;
using System.Collections.Generic;
using System.Linq;
using BucketRepository.Models;

namespace BucketRepository
{
    public class BucketRepository : IBucketRepository, IDisposable
    {
        private static Dictionary<int, BucketDataModel> dataBase;

        public BucketRepository()
        {
            if (dataBase == null)
            {
                dataBase = new Dictionary<int, BucketDataModel>();
            }
        }

        public BucketDataModel Create()
        {
            var lastbucket = dataBase.LastOrDefault();
            int newBucketId = lastbucket.Value == null ? 1 : lastbucket.Key + 1;
            dataBase.Add(newBucketId, new BucketDataModel() { Id = newBucketId, Items = new List<ItemDataModel>() });

            return dataBase[newBucketId];
        }

        public bool Delete(int bucketId)
        {
            return dataBase.Remove(bucketId);
        }

        public void Dispose()
        {
            dataBase = null;
        }

        public IEnumerable<BucketDataModel> Read()
        {
            return dataBase.Values.ToList();
        }

        public BucketDataModel Read(int bucketId)
        {
            return dataBase[bucketId];
        }

        public BucketDataModel Update(int bucketId, BucketDataModel bucketDataModel)
        {
            dataBase[bucketId] = bucketDataModel;
            return dataBase[bucketId];
        }
    }
}
