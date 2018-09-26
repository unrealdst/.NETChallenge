using BucketRepository;
using BucketRepository.Models;
using BucketService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BucketService
{
    public class BucketService : IBucketService
    {
        private IBucketRepository bucketRepository;
        private int bucketId = 1;

        public BucketService(IBucketRepository bucketRepository)
        {
            this.bucketRepository = bucketRepository;
            bucketRepository.Create();
        }

        public void Add(ItemDomainModel domainModelItem)
        {
            if (domainModelItem == null)
            {
                throw new ArgumentNullException("domainModelItem");
            }

            var bucketDataModel = bucketRepository.Read(bucketId);
            if (bucketDataModel == null)
            {
                throw new Exception("Bucket not exist");
            }
            var bucket = this.MapDataToDomain(bucketDataModel);

            domainModelItem.Id = bucket.Items.Count + 1;
            bucket.Items.Add(domainModelItem);

            var result = this.MapDomainToData(bucket);
            bucketRepository.Update(bucketId, result);
        }

        public BucketDomainModel Get()
        {
            BucketDataModel bucketDataModel = bucketRepository.Read(bucketId);
            if (bucketDataModel == null)
            {
                throw new Exception("Bucket not exist");
            }
            return MapDataToDomain(bucketDataModel);
        }

        public void Quantity(int id, int quantity)
        {
            BucketDataModel bucketDataModel = bucketRepository.Read(bucketId);
            if (bucketDataModel == null)
            {
                throw new Exception("Bucket not exist");
            }
            var bucket = this.MapDataToDomain(bucketDataModel);

            if (quantity <= 0)
            {
                bucket.Items = Remove(id, bucket.Items);
            }
            else
            {
                bucket.Items = ChangeAmount(id, quantity, bucket.Items);
            }

            var result = this.MapDomainToData(bucket);
            bucketRepository.Update(bucketId, result);
        }

        private List<ItemDomainModel> ChangeAmount(int id, int quantity, List<ItemDomainModel> items)
        {
            items.Single(x => x.Id == id).Quantity = quantity;
            return items;
        }

        private List<ItemDomainModel> Remove(int id, List<ItemDomainModel> items)
        {
            items.Remove(items.Single(x => x.Id == id));
            return items;
        }

        private BucketDomainModel MapDataToDomain(BucketDataModel bucketDataModel)
        {
            return new BucketDomainModel()
            {
                Items = bucketDataModel.Items.Select(x => new ItemDomainModel() { Id = x.Id, Name = x.Name, PriceForUnit = x.PriceForUnit, Quantity = x.Quantity }).ToList()
            };
        }

        private BucketDataModel MapDomainToData(BucketDomainModel bucketDomainModel)
        {
            return new BucketDataModel()
            {
                Items = bucketDomainModel.Items.Select(x => new ItemDataModel() { Id = x.Id, Name = x.Name, PriceForUnit = x.PriceForUnit, Quantity = x.Quantity }).ToList()
            };
        }
    }
}