using Newtonsoft.Json;
using System.Collections.Generic;

namespace ShopifyChallenge.Models
{
    public class Order
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public Dictionary<string, Item> Items { get; set; }

        public double TotalPrice
        {
            get
            {
                double sum = 0;
                foreach(Item item in Items)
                {
                    sum += item.ItemPrice;
                }
                return sum;
            }
        }
    }
}