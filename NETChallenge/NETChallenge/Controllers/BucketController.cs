using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NETChallenge.Models;

namespace NETChallenge.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BucketController : Controller
    {
        private Bucket Bucket = new Bucket();

        [HttpGet]
        public Bucket Get()
        {
            return Bucket;
        }

        [HttpPost]
        public void Post([FromBody]Item item)
        {
            Bucket.Items.Add(item);
        }

        [HttpPut("{Id}")]
        public void Quantity(int id, int Quantity)
        {
            if(Quantity == 0)
            {
                Bucket.Items.Remove(Bucket.Items.Single(x => x.Id == id));
                return;
            }
            Bucket.Items.Single(x => x.Id == id).Quantity = Quantity;
        }
    }
}