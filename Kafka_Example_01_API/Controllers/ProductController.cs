using Kafka_Example_01_API.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kafka_Example_01_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService         _iproductService;
        private readonly IProductProducer        _productProducer;
        public ProductController(IProductService iproductService, IProductProducer productProducer)
        {
            _iproductService = iproductService;
            _productProducer = productProducer;
        }

        [HttpGet]
        public List<TableProduct> GetProducts()
        {
            return _iproductService.GetProducts();
        }

        [HttpPost("insert-product")]
        public async Task<IActionResult> InsertProduct([FromBody] InsertProductRequest request)
        {
            await _productProducer.ProduceInsertProductAsync(request);
            return Ok(new { Status = "InsertProduct message sent to Kafka topic 'product-input'" });
        }

        [HttpPost("update-quantity")]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityRequest request)
        {
            await _productProducer.ProduceUpdateQuantityAsync(request);
            return Ok(new { Status = "UpdateQuantity message sent to Kafka topic 'product-input'" });
        }

        [HttpPost("update-price")]
        public async Task<IActionResult> UpdatePrice([FromBody] UpdatePriceRequest request)
        {
            await _productProducer.ProduceUpdatePriceAsync(request);
            return Ok(new { Status = "UpdatePrice message sent to Kafka topic 'product-input'" });
        }
    }
}
