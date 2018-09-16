using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using ShopifyChallenge.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopifyChallenge.Controllers
{
    [Route("api/shop/{Id}/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IDocumentClient client;
        private string databaseName = Startup.Configuration["DbConfig:DataBaseName"];
        private string collectionName = Startup.Configuration["DbConfig:CollectionName"];

        public OrderController(IDocumentClient client)
        {
            this.client = client;
        }

        [HttpGet]
        public async Task<List<Order>> GetOrderAsync(string Id)
        {
            Shop res = null;
            try
            {
                res = await client.ReadDocumentAsync<Shop>(UriFactory.CreateDocumentUri(databaseName, collectionName, Id));
            }
            catch
            {
                throw new System.Exception($"Shop with id:{Id} was not found");
            }
            return res.Orders.Values.ToList();
        }


        [HttpGet, Route("{OrderId}")]
        public async Task<Order> GetOrderAsync(string Id, string OrderId)
        {
            Shop res = null;
            try
            {
                res = await client.ReadDocumentAsync<Shop>(UriFactory.CreateDocumentUri(databaseName, collectionName, Id));
            }
            catch
            {
                throw new System.Exception($"Shop with id:{Id} was not found");
            }

            if (res.Orders.ContainsKey(OrderId))
            {
                return res.Orders[OrderId];
            }
            else
            {
                throw new System.Exception($"Order with id:{OrderId} was not found");
            }
        }

        [HttpPut, Route("{OrderId}")]
        public async Task<Order> UpdateOrderAsync(string Id, [FromBody] Order Order)
        {
            Shop shop = await client.ReadDocumentAsync<Shop>(UriFactory.CreateDocumentUri(databaseName, collectionName, Id));
            shop.Orders[Order.Id] = Order;

            //update shop
            await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, shop.Id), shop);
            return Order;
        }

        [HttpPost]
        public async Task<Order> CreateOrderAsync(string Id, [FromBody] Order Order)
        {
            Shop shop = await client.ReadDocumentAsync<Shop>(UriFactory.CreateDocumentUri(databaseName, collectionName, Id));
            shop.Orders[Order.Id] = Order;

            //update shop
            await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, shop.Id), shop);
            return Order;
        }

        [HttpDelete, Route("{OrderId}")]
        public async Task<Order> DeleteOrderAsync(string Id, string OrderId)
        {
            Shop shop = await client.ReadDocumentAsync<Shop>(UriFactory.CreateDocumentUri(databaseName, collectionName, Id));
            var Order = shop.Orders[OrderId];
            shop.Orders[OrderId] = null;

            //update shop
            await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, shop.Id), shop);
            return Order;
        }
    }


}