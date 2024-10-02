using Kafka_Example_01_API.Core.Models;

namespace Kafka_Example_01_API.Core.IRepositories
{
    public interface IProductRepository
    {
        List<TableProduct> GetProducts();

        TableProduct InsertProduct(TableProduct p);

        TableProduct UpdatePrice(int productId, decimal price);

        TableProduct UpdateQuantity(int productId, int quantity, bool increase);
    }
}
