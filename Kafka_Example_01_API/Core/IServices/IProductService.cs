namespace Kafka_Example_01_API.Core.IServices
{
    public interface IProductService
    {
        List<TableProduct> GetProducts();

        TableProduct InsertProduct(TableProduct p);

        TableProduct UpdatePrice(decimal productId, decimal price);

        TableProduct UpdateQuantity(decimal productId, decimal quantity, bool increase);
    }
}
