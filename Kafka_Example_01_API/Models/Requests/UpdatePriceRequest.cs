namespace Kafka_Example_01_API.Models.Requests
{
    public class UpdatePriceRequest
    {
        public string       Key         { get; set; }
        public decimal      ProductId   { get; set; }
        public decimal      Price       { get; set; }
    }
}
