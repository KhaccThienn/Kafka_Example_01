namespace Kafka_Example_01_API.Core.Models.CommandModels
{
    public record InsertProductCommand(string Name, decimal Price, decimal Quantity) : ICommand;
}
