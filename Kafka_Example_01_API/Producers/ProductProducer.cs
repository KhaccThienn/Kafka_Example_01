
namespace Kafka_Example_01_API.Producers
{
    public class ProductProducer : IProductProducer, IDisposable
    {
        private readonly IProducer<string, string>  _producer;
        private readonly string                     _topic;

        public ProductProducer(IConfiguration configuration)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers      = configuration["KafkaConfig:BootstrapServers"],
                AllowAutoCreateTopics = true
            };

            _topic    = configuration["KafkaConfig:ProductTopic"];
            _producer = new ProducerBuilder<string, string>(producerConfig).Build();
        }

        private async Task SendMessageAsync(Message<string, string> message)
        {
            try
            {
                var deliveryResult = await _producer.ProduceAsync(_topic, message);
                await Console.Out.WriteLineAsync($"[Producer] Delivered '{deliveryResult.Value}' to '{deliveryResult.TopicPartitionOffset}'");
            }
            catch (ProduceException<string, string> ex)
            {
                await Console.Out.WriteLineAsync($"Error producing message: {ex.Error.Reason}");
            }
        }

        public void Dispose()
        {
            _producer?.Dispose();
        }

        public async Task ProduceInsertProductAsync(InsertProductRequest product)
        {
            var message = new Message<string, string>
            {
                Key     = string.Empty, // Key là empty hoặc null
                Value   = JsonSerializer.Serialize(product),
                Headers = new Headers
                {
                    { "eventname", Encoding.UTF8.GetBytes("InsertProduct") }
                }
            };

            await SendMessageAsync(message);
        }

        public async Task ProduceUpdatePriceAsync(UpdatePriceRequest request)
        {
            var value = new UpdatePriceDTO
            {
                ProductId = request.ProductId,
                Price     = request.Price,
            };
            var message = new Message<string, string>
            {
                Key     = request.Key,
                Value   = JsonSerializer.Serialize(value),
                Headers = new Headers
                {
                    { "eventname", Encoding.UTF8.GetBytes("UpdatePrice") }
                }
            };

            await SendMessageAsync(message);
        }

        public async Task ProduceUpdateQuantityAsync(UpdateQuantityRequest request)
        {
            var value = new UpdateQuantityDTO
            {
                ProductId = request.ProductId,
                Quantity  = request.Quantity,
                Increase  = request.Increase
            };

            var message = new Message<string, string>
            {
                Key     = request.ProductId.ToString(),
                Value   = JsonSerializer.Serialize(value),
                Headers = new Headers
                {
                    { "eventname", Encoding.UTF8.GetBytes("UpdateQuantity") }
                }
            };

            await SendMessageAsync(message);
        }
    }
}
