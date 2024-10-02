namespace Kafka_Example_01_API.Core.DTOs
{
    public class UpdateQuantityDTO
    {
        public decimal  ProductId { get; set; }
        public decimal  Quantity  { get; set; }
        public bool     Increase  { get; set; }
    }
}
