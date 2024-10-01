using Kafka_Example_01_API.Core.InMemories;
using Kafka_Example_01_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Kafka_Example_01_API.Core
{
    public class ProductMemorySeedAsync
    {
        public async Task SeedAsync(ProductMemory memory, ApplicationDbContext dbContext)
        {
            var products = await dbContext.TableProducts.ToListAsync();
            if (products.Count > 0)
            {
                foreach (var product in products)
                {
                    memory.Memory.Add(product.Id.ToString(), product);
                }
            }
        }
    }
}
