namespace Kafka_Example_01_API.Commands.Handlers
{
    public class UpdateProductQuantityCommandHandler : ICommandHandler<UpdateQuantityCommand>
    {
        private readonly IProductPersistenceService _service;
        public UpdateProductQuantityCommandHandler(IProductPersistenceService service)
        {
            _service = service;
        }
        public async Task HandleAsync(UpdateQuantityCommand command)
        {
            await _service.UpdateQuantity(command.ProductId, command.Quantity, command.Increase);
        }
    }
}
