using Kafka_Example_01_API.Core.Models;

namespace Kafka_Example_01_API.Core.IServices
{
    public interface IProductService
    {
        List<TableProduct> GetProducts();

        TableProduct InsertProduct(TableProduct p);

        TableProduct UpdatePrice(string key, decimal productId, decimal price);

        TableProduct UpdateQuantity(string key, decimal productId, decimal quantity, bool increase);
    }
}
