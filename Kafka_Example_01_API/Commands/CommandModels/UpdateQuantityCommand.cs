namespace Kafka_Example_01_API.Commands.CommandModels
{
    public record UpdateQuantityCommand(decimal ProductId, decimal Quantity, bool Increase) : ICommand;
}
