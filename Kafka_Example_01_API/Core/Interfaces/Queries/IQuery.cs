namespace Kafka_Example_01_API.Core.Interfaces.Queries
{
    public interface IQuery
    {
    }

    public interface IQuery<TResult> : IQuery
    {
    }
}
