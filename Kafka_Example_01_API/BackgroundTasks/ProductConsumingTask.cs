namespace Kafka_Example_01_API.BackgroundTasks
{
    public class ProductConsumingTask : IConsumingTask<string, string>
    {
        private readonly ILogger<ProductConsumingTask> _logger;
        private readonly IProductService               _productService;
        public ProductConsumingTask(IProductService    productService, ILogger<ProductConsumingTask> logger)
        {
            _productService = productService;
            _logger         = logger;
        }
        public async Task ExecuteAsync(ConsumeResult<string, string> result)
        {
            var productEvent = "";
            foreach (var header in result.Message.Headers)
            {
                productEvent = Encoding.UTF8.GetString(header.GetValueBytes());
            }

            _logger.LogInformation(
                $"Consuming message from topic: {result.Topic}, Partition: {result.Partition}, Offset: {result.Offset}, Key: {result.Message.Key}");

            switch (productEvent)
            {
                case "InsertProduct":
                    var product = JsonSerializer.Deserialize<TableProduct>(result.Message.Value);

                    Guid newGuid = Guid.NewGuid();
                    int id       = Math.Abs(newGuid.GetHashCode());
                    product.Id   = id;

                    _logger.LogInformation("Inserting new product: {@Product}", product);

                    _productService.InsertProduct(product);
                    break;
                case "UpdateQuantity":

                    _logger.LogInformation(result.Message.Key);
                    _logger.LogInformation(result.Message.Value);

                    var prod = JsonSerializer.Deserialize<UpdateQuantityDTO>(result.Message.Value);
                    var key = result.Message.Key;

                    _logger.LogInformation("Updating quantity for product ID: {ProductId}, New Quantity: {Quantity}, Increase: {Increase}",
                        prod.ProductId, prod.Quantity, prod.Increase);

                    _productService.UpdateQuantity(key, prod.ProductId, prod.Quantity, prod.Increase);
                    break;
                case "UpdatePrice":
                    var p = JsonSerializer.Deserialize<UpdatePriceDTO>(result.Message.Value);
                    var k = result.Message.Key;
                    _logger.LogInformation("Updating price for product ID: {ProductId}, New Price: {Price}", p.ProductId, p.Price);
                    _productService.UpdatePrice(k, p.ProductId, p.Price);
                    break;
                default:
                    _logger.LogWarning("Received unknown event: {Event}", productEvent);
                    break;
            }
        }
    }
}
