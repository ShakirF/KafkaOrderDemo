using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
         private readonly  IConfiguration _configuration;

        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            var kafkaConfig = new ProducerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"]
            };

            using var producer = new ProducerBuilder<Null, string>(kafkaConfig).Build();
            var json = JsonSerializer.Serialize(order);
            await producer.ProduceAsync("order-topic", new Message<Null, string> { Value = json });

            return Ok("Order published to Kafka");
        }
    }

    public record Order(Guid Id, string CustomerName, string Product, int Quantity);
}
