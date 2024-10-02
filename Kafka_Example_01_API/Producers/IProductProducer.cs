namespace Kafka_Example_01_API.Producers
{
    public interface IProductProducer
    {
        Task ProduceInsertProductAsync(InsertProductRequest product);
        Task ProduceUpdateQuantityAsync(UpdateQuantityRequest request);
        Task ProduceUpdatePriceAsync(UpdatePriceRequest request);
    }
}
