
using Kafka_Example_01_API.Core.Models;

namespace Kafka_Example_01_API.Core.InMemories
{
    public class ProductMemory
    {
        public Dictionary<string, TableProduct> Memory { get; set; }

        public ProductMemory()
        {
            Memory = new Dictionary<string, TableProduct>();
        }

    }
}
