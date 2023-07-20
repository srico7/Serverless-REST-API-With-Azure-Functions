using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore; 

namespace ServerlessRESTAzure
{
    public class ProductsGetAllCreate
    {
        private readonly AppDbContext _ctx; 

        public ProductsGetAllCreate(AppDbContext context)
        {
            _ctx = context; // Initialize the AppDbContext field through constructor injection
        }

        [FunctionName("ProductsGetAllCreate")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "products")] HttpRequest req)
        {
            if (req.Method == HttpMethods.Post)
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var product = JsonConvert.DeserializeObject<Product>(requestBody);
                _ctx.Products.Add(product);
                await _ctx.SaveChangesAsync();
                return new CreatedResult("/products", product);
            }

            var products = await _ctx.Products.ToListAsync(); 
            return new OkObjectResult(products);
        }
    }
}

