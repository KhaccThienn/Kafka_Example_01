namespace Kafka_Example_01_API.Core.Models.CommandModels
{
    public record UpdatePriceCommand(decimal ProductId, decimal Price) : ICommand;
}
