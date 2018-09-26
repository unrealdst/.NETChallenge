using System.Linq;
using BucketService;
using BucketService.Models;
using Microsoft.AspNetCore.Mvc;
using NETChallenge.Models;

namespace NETChallenge.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BucketController : Controller
    {
        private readonly IBucketService bucketService;

        public BucketController()
        {
            var bucketRepository = new BucketRepository.BucketRepository();
            bucketService = new BucketService.BucketService(bucketRepository);
        }

        [HttpGet]
        public BucketViewModel Get()
        {
            BucketDomainModel bucket = bucketService.Get();
            return new BucketViewModel()
            {
                Items = bucket.Items.Select(x => new ItemViewModel() { Id = x.Id, Name = x.Name, PriceForUnit = x.PriceForUnit, Quantity = x.Quantity }).ToList()
            };
        }

        [HttpPost]
        public void Post([FromBody]ItemViewModel item)
        {
            if (ModelState.IsValid)
            {
                var domainModelItem = new ItemDomainModel()
                {
                    Name = item.Name,
                    PriceForUnit = item.PriceForUnit,
                    Quantity = item.Quantity
                };

                bucketService.Add(domainModelItem);
            }
        }

        [HttpPut("{Id}")]
        public void Quantity(int id, int quantity)
        {
            bucketService.Quantity(id, quantity);
        }
    }
}