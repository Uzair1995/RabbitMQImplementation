using RabbitMQ.Client;
using System;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RabbitMQProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                var factory = new ConnectionFactory() { Uri = new Uri("amqp://guest:guest@localhost:5672") };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "QueueName", durable: true, exclusive: false, autoDelete: false, arguments: null);

                    DataModel message = new DataModel { ID = i, Name = "New Name", Message = "This is a new message" };
                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                    channel.BasicPublish(exchange: "", routingKey: "QueueName", basicProperties: null, body: body);
                    Console.WriteLine(" [x] Sent Msg: {0}", message.ID);
                }
                Task.Delay(1000);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
