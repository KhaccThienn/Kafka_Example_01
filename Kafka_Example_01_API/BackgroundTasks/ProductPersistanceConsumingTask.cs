namespace Kafka_Example_01_API.BackgroundTasks
{
    public class ProductPersistanceConsumingTask : IConsumingTask<string, string>
    {
        private readonly ILogger<ProductPersistanceConsumingTask>         _logger;
        private readonly ICommandHandler<InsertProductCommand>            _insertProductCommandHandler;
        private readonly ICommandHandler<UpdatePriceCommand>              _updatePriceProductCommandHandler;
        private readonly ICommandHandler<UpdateQuantityCommand>           _updateQuantityProductCommandHandler;

        public ProductPersistanceConsumingTask(
            ILogger<ProductPersistanceConsumingTask> logger,
            ICommandHandler<InsertProductCommand>    insertProductCommandHandler,
            ICommandHandler<UpdatePriceCommand>      updatePriceProductCommandHandler,
            ICommandHandler<UpdateQuantityCommand>   updateQuantityProductCommandHandler
            )
        {
            _logger                              = logger;
            _insertProductCommandHandler         = insertProductCommandHandler;
            _updatePriceProductCommandHandler    = updatePriceProductCommandHandler;
            _updateQuantityProductCommandHandler = updateQuantityProductCommandHandler;
        }
        public async Task ExecuteAsync(ConsumeResult<string, string> result)
        {
            var productEvent = "";

            try
            {
                foreach (var header in result.Message.Headers)
                {
                    productEvent = Encoding.UTF8.GetString(header.GetValueBytes());
                }

                _logger.LogInformation("Consumed message from topic {Topic}, partition {Partition}, offset {Offset} with event {Event}",
                        result.Topic, result.Partition, result.Offset, productEvent);

                switch (productEvent)
                {
                    case "InsertProduct":
                        var product = JsonSerializer.Deserialize<TableProduct>(result.Message.Value);

                        TableProduct p = new TableProduct();

                        p.Price      = product.Price;
                        p.Name       = product.Name;
                        p.Quantity   = product.Quantity;

                        await _insertProductCommandHandler.HandleAsync(new InsertProductCommand(p.Name, p.Price, p.Quantity));
                        
                        _logger.LogInformation("[PersistanceConsuming] Inserted product: {@Product}", product);
                        
                        break;

                    case "UpdateQuantity":
                        var prod = JsonSerializer.Deserialize<UpdateQuantityCommand>(result.Message.Value);

                        await _updateQuantityProductCommandHandler.HandleAsync(new UpdateQuantityCommand(prod.ProductId ,prod.Quantity, prod.Increase));

                        _logger.LogInformation("[PersistanceConsuming] Updated quantity for product ID {ProductId} to {Quantity}, Increase {Increase}",
                                prod.ProductId, prod.Quantity, prod.Increase);

                        break;

                    case "UpdatePrice":
                        var prd = JsonSerializer.Deserialize<UpdatePriceCommand>(result.Message.Value);

                        await _updatePriceProductCommandHandler.HandleAsync(new UpdatePriceCommand(prd.ProductId, prd.Price));

                        _logger.LogInformation("Updated price for product ID {ProductId} to {Price}",
                                prd.ProductId, prd.Price);
                        break;
                    default:
                        _logger.LogWarning($"Received unknown event: {productEvent}");
                        break;
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, $"Failed to deserialize message value: {result.Message.Value}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while processing message with event {productEvent}");
            }
        }
    }
}
