namespace Kafka_Example_01_API.Services
{
    public class ProductService : IProductService
    {
        private readonly IKafkaProducerManager   _producerManager;
        private readonly ILogger<ProductService> _logger;
        private readonly ProductMemory           _inMem;

        public ProductService(IKafkaProducerManager manager, ILogger<ProductService> logger, ProductMemory inMem)
        {
            _producerManager = manager;
            _logger          = logger;
            _inMem           = inMem;
        }

        public List<TableProduct> GetProducts()
        {
            List<TableProduct> productList = new List<TableProduct>();
            try
            {
                productList = _inMem.Memory.Values.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
            return productList;
        }

        public TableProduct InsertProduct(TableProduct p)
        {
            try
            {
                _inMem.Memory.Add(p.Id.ToString(), p);

                var kafkaProducer = _producerManager.GetProducer<string, string>("1");
                var message       = new Message<string, string>
                {
                    Value   = JsonSerializer.Serialize(p),
                    Headers = new Headers
                    {
                        { "eventname", Encoding.UTF8.GetBytes("InsertProduct") },
                    }
                };

                kafkaProducer.Produce(message);
                return p;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;

            }
        }

        public TableProduct UpdateQuantity(string key, decimal productId, decimal quantity, bool increase)
        {
            var p = _inMem.Memory.FirstOrDefault(x => x.Key == key).Value;

            if (p != null)
            {
                _inMem.Memory.TryGetValue(key, out p);
                if (increase)
                {
                    p.Quantity += quantity;
                }
                else
                {
                    if (p.Quantity < quantity)
                    {
                        p.Quantity = p.Quantity;
                        _logger.LogError("Cannot Update Quantity");
                    }
                    else
                    {
                        p.Quantity -= quantity;
                    }
                }
                
                var prod = new UpdateQuantityDTO
                {
                    ProductId = productId,
                    Quantity  = quantity,
                    Increase  = increase
                };

                var kafkaProducer = _producerManager.GetProducer<string, string>("1");
                var message       = new Message<string, string>
                {
                    Value   = JsonSerializer.Serialize(prod),
                    Headers = new Headers
                    {
                        { "eventname", Encoding.UTF8.GetBytes("UpdateQuantity") },
                    }
                };

                kafkaProducer.Produce(message);

                p.Id = productId;

                _inMem.Memory.Remove(key);
                _inMem.Memory.Add(p.Id.ToString(), p);
            }
            else
            {
                _logger.LogError("ID Not Found");
            }
            return p;
        }

        public TableProduct UpdatePrice(string key, decimal productId, decimal price)
        {
            var p = _inMem.Memory.FirstOrDefault(x => x.Key == key).Value;

            _inMem.Memory.TryGetValue(key, out p);
            if (p != null)
            {
                if (price <= 0)
                {
                    _logger.LogError("Price must be greater than 0");
                }
                else
                {
                    p.Price = price;
                    _inMem.Memory.Remove(productId.ToString());
                    _inMem.Memory.Add(p.Id.ToString(), p);

                    var prod = new UpdatePriceDTO
                    {
                        ProductId = productId,
                        Price     = price
                    };

                    var kafkaProducer = _producerManager.GetProducer<string, string>("1");
                    var message       = new Message<string, string>
                    {
                        Value   = JsonSerializer.Serialize(prod),
                        Headers = new Headers
                    {
                        { "eventname", Encoding.UTF8.GetBytes("UpdatePrice") },
                    }
                    };

                    kafkaProducer.Produce(message);

                    p.Id = productId;

                    _inMem.Memory.Remove(key);
                    _inMem.Memory.Add(p.Id.ToString(), p);
                }
            }
            else
            {
                _logger.LogError("Not Found Item in Memory");
            }
            return p;

        }


    }
}
