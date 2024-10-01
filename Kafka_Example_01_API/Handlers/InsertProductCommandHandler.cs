using static Confluent.Kafka.ConfigPropertyNames;

namespace Kafka_Example_01_API.Handlers
{
    public class InsertProductCommandHandler : ICommandHandler<InsertProductCommand>
    {
        private readonly IProductPersistenceService _service;
        public InsertProductCommandHandler(IProductPersistenceService service)
        {
            _service = service;
        }
        public async Task HandleAsync(InsertProductCommand command)
        {
            var product = new TableProduct
            {
                Name     = command.Name,
                Price    = command.Price,
                Quantity = command.Quantity
            };
            await _service.InsertProduct(product);
        }
    }
}
