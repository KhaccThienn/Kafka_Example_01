using Kafka_Example_01_API.Core.Models;

namespace Kafka_Example_01_API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void LoadProductMemoryData(this WebApplication app)
        {
            app.LoadDataToMemory<ProductMemory, ApplicationDbContext>((productInMe, dbContext) =>
            {
                new ProductMemorySeedAsync().SeedAsync(productInMe, dbContext).Wait();
            });
        }

        public static void UseCustomKafkaMessageBus(this WebApplication app)
        {
            app.UseKafkaMessageBus(mess =>
            {
                mess.RunConsumerAsync("0");
                mess.RunConsumerAsync("1");
            });
        }
    }
}
