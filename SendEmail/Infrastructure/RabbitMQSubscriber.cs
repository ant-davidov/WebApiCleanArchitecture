// Infrastructure
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using SendEmail.Application.Interfaces;
using SendEmail.Domain;
using System.Text;
using Newtonsoft.Json;
using SendEmail.Infrastructure.Models;

public class RabbitMQSubscriber : IMessageQueueSubscriber
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string queueName;
    private readonly Action<Message> _messageHandler;
   
    public RabbitMQSubscriber(RabbitMqConfig config, Action<Message> messageHandler)
    {
        _messageHandler = messageHandler;
        queueName = config.QueueName;
        var factory = new ConnectionFactory();
        factory.Uri = new Uri(config.Uri);
       

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void Subscribe()
    {
        _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            byte[] body = ea.Body.ToArray();
            var message = JsonConvert.DeserializeObject<Message>(  Encoding.UTF8.GetString(body));
            _messageHandler(message);
        };

        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
    }
}