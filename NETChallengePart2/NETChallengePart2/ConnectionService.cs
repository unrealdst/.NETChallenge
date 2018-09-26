using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace NetChallengePart2
{
    public class ConnectionService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string apiUrl = "http://localhost:62713/api";

        internal async System.Threading.Tasks.Task<BucketViewModel> GetAsync()
        {
            HttpResponseMessage msg = await client.GetAsync(apiUrl + "/Bucket");

            BucketViewModel bucket = JsonConvert.DeserializeObject<BucketViewModel>(await msg.Content.ReadAsStringAsync());
            return bucket;
        }

        internal void Add(ItemViewModel item)
        {
            string serializedItem = JsonConvert.SerializeObject(item);
            client.PostAsync(apiUrl + "/Bucket", new StringContent(serializedItem, Encoding.UTF8, "application/json"));
        }

        internal void Quantity(int itemId, int quantity)
        {
            var dictonary = new Dictionary<string, string>();
            dictonary.Add("quantity", quantity.ToString());
            var request = new HttpRequestMessage(HttpMethod.Put, apiUrl + $"/Bucket/{itemId}") { Content = new FormUrlEncodedContent(dictonary) };
            client.SendAsync(request);
        }
    }
}
