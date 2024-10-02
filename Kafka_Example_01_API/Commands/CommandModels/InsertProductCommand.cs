namespace Kafka_Example_01_API.Commands.CommandModels
{
    public record InsertProductCommand(decimal id, string Name, decimal Price, decimal Quantity) : ICommand;
}
