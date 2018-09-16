using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ShopifyChallenge
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<IDocumentClient>(new DocumentClient(new System.Uri(Configuration["DbConfig:URI"]), Configuration["DbConfig:PrimaryKey"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, IDocumentClient client)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            // Init documentdb database
            Database shopifychallenge = await client.CreateDatabaseIfNotExistsAsync(new Database { Id = Configuration["DbConfig:DataBaseName"] });
            await client.CreateDocumentCollectionIfNotExistsAsync(shopifychallenge.SelfLink, new DocumentCollection { Id = Configuration["DbConfig:CollectionName"] });
        }
    }
}
