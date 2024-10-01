namespace Kafka_Example_01_API.Core.Models.CommandModels
{
    public record UpdateQuantityCommand(decimal ProductId, decimal Quantity, bool Increase) : ICommand;
}
