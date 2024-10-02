namespace Kafka_Example_01_API.Commands.CommandModels
{
    public record UpdatePriceCommand(decimal ProductId, decimal Price) : ICommand;
}
