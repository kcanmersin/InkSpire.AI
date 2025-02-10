using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

public class RabbitMQPublisher : IMessagePublisher
{
    private readonly IModel _channel;

    public RabbitMQPublisher(IConnection connection)
    {
        _channel = connection.CreateModel();
        _channel.QueueDeclare(queue: "book_created", durable: true, exclusive: false, autoDelete: false, arguments: null);
    }

    public void Publish<T>(string queueName, T message)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
    }
}
