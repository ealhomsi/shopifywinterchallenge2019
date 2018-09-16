using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using ShopifyChallenge.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopifyChallenge.Controllers
{
    [Route("api/shop/{Id}/order/{OrderId}/item")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private IDocumentClient client;
        private string databaseName = Startup.Configuration["DbConfig:DataBaseName"];
        private string collectionName = Startup.Configuration["DbConfig:CollectionName"];

        public ItemController(IDocumentClient client)
        {
            this.client = client;
        }

        [HttpGet]
        public async Task<List<Item>> GetItemAsync(string Id, string OrderId)
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
            return res.Orders[OrderId].Items.Values.ToList();
        }


        [HttpGet, Route("{ItemId}")]
        public async Task<Item> GetItemAsync(string Id, string OrderId, string ItemId)
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

            if (res.Orders[OrderId].Items.ContainsKey(ItemId))
            {
                return res.Orders[OrderId].Items[ItemId];
            }
            else
            {
                throw new System.Exception($"Item with id:{ItemId} was not found");
            }
        }

        [HttpPut, Route("{ItemId}")]
        public async Task<Item> UpdateItemAsync(string Id, string OrderId, [FromBody] Item Item)
        {
            Shop shop = await client.ReadDocumentAsync<Shop>(UriFactory.CreateDocumentUri(databaseName, collectionName, Id));
            shop.Orders[OrderId].Items[Item.Id] = Item;

            //update shop
            await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, shop.Id), shop);
            return Item;
        }

        [HttpPost]
        public async Task<Item> CreateItemAsync(string Id, string OrderId, [FromBody] Item Item)
        {
            Shop shop = await client.ReadDocumentAsync<Shop>(UriFactory.CreateDocumentUri(databaseName, collectionName, Id));
            shop.Orders[OrderId].Items[Item.Id] = Item;

            //update shop
            await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, shop.Id), shop);
            return Item;
        }

        [HttpDelete, Route("{ItemId}")]
        public async Task<Item> DeleteItemAsync(string Id, string OrderId, string ItemId)
        {
            Shop shop = await client.ReadDocumentAsync<Shop>(UriFactory.CreateDocumentUri(databaseName, collectionName, Id));
            var Item = shop.Orders[OrderId].Items[ItemId];
            shop.Orders[OrderId].Items[Item.Id] = null;

            //update shop
            await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, shop.Id), shop);
            return Item;
        }
    }


}