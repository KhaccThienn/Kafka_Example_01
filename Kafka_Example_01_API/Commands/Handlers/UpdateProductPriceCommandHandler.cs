namespace Kafka_Example_01_API.Commands.Handlers
{
    public class UpdateProductPriceCommandHandler : ICommandHandler<UpdatePriceCommand>
    {
        private readonly IProductPersistenceService _service;
        public UpdateProductPriceCommandHandler(IProductPersistenceService service)
        {
            _service = service;
        }
        public async Task HandleAsync(UpdatePriceCommand command)
        {
            await _service.UpdatePrice(command.ProductId, command.Price);
        }
    }
}
