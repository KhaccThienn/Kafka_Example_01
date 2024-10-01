namespace Kafka_Example_01_API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext         _dbContext;
        private readonly IConfiguration               _configuration;
        private readonly IKafkaProducerManager        _producerManager;
        private readonly ILogger<ProductService>      _logger;
        private readonly ProductMemory                _inMem;

        public ProductRepository(ApplicationDbContext dbContext, IConfiguration config, IKafkaProducerManager manager, ILogger<ProductService> logger, ProductMemory inMem)
        {
            _dbContext = dbContext;
            _configuration = config;
            _producerManager = manager;
            _logger = logger;
            _inMem = inMem;
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
                var message = new Message<string, string>
                {
                    Value = JsonSerializer.Serialize(p),
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

        public TableProduct UpdatePrice(int productId, decimal price)
        {
            var p = _inMem.Memory.FirstOrDefault(x => x.Key == productId.ToString()).Value;

            TableProduct product = new TableProduct();

            product.Id = p.Id;
            product.Name = p.Name;
            product.Quantity = p.Quantity;
            product.Price = p.Price;

            _inMem.Memory.TryGetValue(productId.ToString(), out p);
            if (price <= 0)
            {
                _logger.LogError("Price must be greater than 0");
            }
            else
            {
                p.Price = price;
                var kafkaProducer = _producerManager.GetProducer<string, string>("1");
                var message = new Message<string, string>
                {
                    Value = JsonSerializer.Serialize(p),
                    Headers = new Headers
                    {
                        { "eventname", Encoding.UTF8.GetBytes("UpdatePrice") },
                    }
                };
                _inMem.Memory.Remove(productId.ToString());
                _inMem.Memory.Add(p.Id.ToString(), p);
                kafkaProducer.Produce(message);
            }
            

            return p;
        }

        public TableProduct UpdateQuantity(int productId, int quantity, bool increase)
        {
            var p = _inMem.Memory.FirstOrDefault(x => x.Key == productId.ToString()).Value;

            TableProduct product = new TableProduct();

            product.Id = p.Id;
            product.Name = p.Name;
            product.Quantity = p.Quantity;
            product.Price = p.Price;

            if (p != null)
            {
                //_inMem.Memory.TryGetValue(productId.ToString(), out p);
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
                        var kafkaProducer = _producerManager.GetProducer<string, string>("1");
                        var message = new Message<string, string>
                        {
                            Value = JsonSerializer.Serialize(p),
                            Headers = new Headers
                            {
                                { "eventname", Encoding.UTF8.GetBytes("UpdateQuantity") },
                            }
                        };

                        
                        kafkaProducer.Produce(message);
                    }
                }
                p.Id = product.Id;
                _inMem.Memory.Remove(productId.ToString());
                _inMem.Memory.Add(p.Id.ToString(), p);
                return p;
            }
            else
            {
                throw new Exception("Product not found");
            }
        }
    }
}
