using Kafka_Example_01_API.Core.IServices;
using Microsoft.EntityFrameworkCore;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Kafka_Example_01_API.Services
{
    public class ProductPersistenceService : IProductPersistenceService
    {
        private readonly IServiceScopeFactory                 _scopeFactory;
        private readonly ILogger<ProductConsumingTask>        _logger;
        public ProductPersistenceService(IServiceScopeFactory scopeFactory, ILogger<ProductConsumingTask> logger)
        {
            _scopeFactory = scopeFactory;
            _logger       = logger;
        }

        public async Task<TableProduct> InsertProduct(TableProduct p)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                // Use dbContext here
                try
                {
                    dbContext.TableProducts.AddAsync(p);
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return p;
        }

        public async Task<TableProduct> UpdatePrice(decimal productId, decimal price)
        {

            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            // Use dbContext here
            var product = await dbContext.TableProducts.FirstOrDefaultAsync(p => p.Id == productId);

            bool flag = true;
            product.Id = productId;
            product.Price = price;

            if (price < 0)
            {
                flag = false;
            }
            if (flag)
            {
                try
                {
                    dbContext.TableProducts.Update(product);
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return product;
        }

        public async Task<TableProduct> UpdateQuantity(decimal productId, decimal quantity, bool Increase)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var product = await dbContext.TableProducts.FirstOrDefaultAsync(p => p.Id == productId);

            bool flag = true;
            if (product != null)
            {
                if (Increase)
                {
                    product.Quantity += quantity;
                }
                else
                {
                    product.Quantity -= quantity;
                    if (quantity > product.Quantity)
                    {
                        flag = false;
                    }
                }

                if (flag)
                {
                    try
                    {
                        dbContext.TableProducts.Update(product);
                        await dbContext.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            else
            {
                _logger.LogError("Product Not Found");
            }


            return product;
        }


    }
}
