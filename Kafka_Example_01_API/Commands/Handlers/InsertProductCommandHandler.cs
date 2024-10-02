namespace Kafka_Example_01_API.Commands.Handlers
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
                Id       = command.id,
                Name     = command.Name,
                Price    = command.Price,
                Quantity = command.Quantity
            };
            await _service.InsertProduct(product);
        }
    }
}
