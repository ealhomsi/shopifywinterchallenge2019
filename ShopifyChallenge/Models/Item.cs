using Newtonsoft.Json;

namespace ShopifyChallenge.Models
{
    public class Item
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public double ItemPrice { get
            {
                return Product.Price * Quantity;
            }
        }
    }
}