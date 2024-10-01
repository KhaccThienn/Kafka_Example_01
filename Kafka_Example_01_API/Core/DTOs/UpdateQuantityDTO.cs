namespace Kafka_Example_01_API.Core.DTOs
{
    public class UpdateQuantityDTO
    {
        public int  ProductId { get; set; }
        public int  Quantity  { get; set; }
        public bool Increase  { get; set; }
    }
}
