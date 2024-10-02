namespace Kafka_Example_01_API.Models.Requests
{
    public class InsertProductRequest
    {
        public string    Name       { get; set; }
        public decimal   Price      { get; set; }
        public decimal   Quantity   { get; set; }
    }
}
