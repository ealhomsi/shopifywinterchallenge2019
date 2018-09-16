using Newtonsoft.Json;
using System.Collections.Generic;

namespace ShopifyChallenge.Models
{
    public class Shop 
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public Dictionary<string, Product> Products { get; set; }
        public Dictionary<string, Order> Orders { get; set; }
    }
}
