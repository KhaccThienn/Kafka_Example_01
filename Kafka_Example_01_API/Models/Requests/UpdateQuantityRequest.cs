namespace Kafka_Example_01_API.Models.Requests
{
    public class UpdateQuantityRequest
    {
        public string       Key         { get; set; }
        public decimal      ProductId   { get; set; }
        public decimal      Quantity    { get; set; }
        public bool         Increase    { get; set; }
    }
}
