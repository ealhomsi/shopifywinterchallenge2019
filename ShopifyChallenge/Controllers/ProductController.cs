using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using ShopifyChallenge.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopifyChallenge.Controllers
{
    [Route("api/shop/{Id}/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IDocumentClient client;
        private string databaseName = Startup.Configuration["DbConfig:DataBaseName"];
        private string collectionName = Startup.Configuration["DbConfig:CollectionName"];

        public ProductController(IDocumentClient client)
        {
            this.client = client;
        }

        [HttpGet]
        public async Task<List<Product>> GetProductsAsync(string Id)
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
            return res.Products.Values.ToList();
        }


        [HttpGet, Route("{Name}")]
        public async Task<Product> GetProductAsync(string Id, string Name)
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

            if (res.Products.ContainsKey(Name))
            {
                return res.Products[Name];
            }
            else
            {
                throw new System.Exception($"Product with name:{Name} was not found");
            }
        }

        [HttpPut, Route("{Name}")]
        public async Task<Product> UpdateProductAsync(string Id, [FromBody] Product product)
        {
            Shop shop = await client.ReadDocumentAsync<Shop>(UriFactory.CreateDocumentUri(databaseName, collectionName, Id));
            shop.Products[product.Name] = product;

            //update shop
            await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, shop.Id), shop);
            return product;
        }

        [HttpPost]
        public async Task<Product> CreateProductAsync(string Id, [FromBody] Product product)
        {
            Shop shop = await client.ReadDocumentAsync<Shop>(UriFactory.CreateDocumentUri(databaseName, collectionName, Id));
            shop.Products[product.Name] = product;

            //update shop
            await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, shop.Id), shop);
            return product;
        }

        [HttpDelete, Route("{Name}")]
        public async Task<Product> DeleteProductAsync(string Id, string Name)
        {
            Shop shop = await client.ReadDocumentAsync<Shop>(UriFactory.CreateDocumentUri(databaseName, collectionName, Id));
            var product = shop.Products[Name];
            shop.Products[Name] = null;

            //update shop
            await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, shop.Id), shop);
            return product;
        }
    }


}