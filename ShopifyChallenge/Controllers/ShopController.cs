using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using ShopifyChallenge.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ShopifyChallenge.Controllers
{
    [Route("api/shop")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private IDocumentClient client;
        private string databaseName = Startup.Configuration["DbConfig:DataBaseName"];
        private string collectionName = Startup.Configuration["DbConfig:CollectionName"];

        public ShopController(IDocumentClient client)
        {
            this.client = client;
        }

        [HttpGet]
        public IOrderedQueryable<Shop> GetShopsAsync()
        {
            return client.CreateDocumentQuery<Shop>(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName));
        }


        [HttpGet, Route("{Id}")]
        public async Task<Shop> GetShopAsync(string Id)
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
            return res;
        }

        [HttpPut, Route("{Id}")]
        public async Task<Document> UpdateShopAsync([FromBody] Shop shop)
        {
            return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, shop.Id), shop);
        }

        [HttpPost]
        public async Task<Document> CreateShopAsync([FromBody] Shop shop)
        {
            return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), shop);
        }

        [HttpDelete, Route("{Id}")]
        public async Task<Document> DeleteShopAsync(string Id)
        {
            Document res = null;
            try
            {
                res = await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, Id));
            }
            catch
            {
                throw new System.Exception($"Shop with id:{Id} was not found");
            }
            return res;
        }
    }


}