namespace Kafka_Example_01_API.Core.IServices
{
    public interface IProductPersistenceService
    {
        Task<TableProduct> InsertProduct(TableProduct p);
        Task<TableProduct> UpdateQuantity(decimal productId, decimal quantity, bool Increase);
        Task<TableProduct> UpdatePrice(decimal productId, decimal price);
    }
}
