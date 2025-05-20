using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotificationService
{
    public class KafkaConsumerService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "notification-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe("order-topic");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var cr = consumer.Consume(stoppingToken);
                    var order = JsonSerializer.Deserialize<Order>(cr.Message.Value);
                    Console.WriteLine($"📨 New Order Received: {order}");
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }

            consumer.Close();
        }

        public record Order(Guid Id, string CustomerName, string Product, int Quantity);
    }
}
