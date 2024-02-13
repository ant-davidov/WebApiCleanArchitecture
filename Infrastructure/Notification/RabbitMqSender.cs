using Application.Contracts.Infrastructure;
using Application.DTOs.Exceptions;
using Application.Models;
using Infrastructure.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Notification
{
    internal class RabbitMqSender : INotificationSender
    {
        private readonly string queueName;
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly EmailSettings _emailSettings;
        public RabbitMqSender(IOptions<RabbitMqConfig> rabbitMqConfig, IOptions<EmailSettings> emailSettings)
        {
            queueName = rabbitMqConfig.Value.QueueName;
            _emailSettings = emailSettings.Value;
            var factory = new ConnectionFactory() { HostName = rabbitMqConfig.Value.HostName };
            factory.UserName = rabbitMqConfig.Value.Username;
            factory.Password = rabbitMqConfig.Value.Password;
            try
            {
                connection = factory.CreateConnection();
                channel = connection.CreateModel();
                channel.QueueDeclare(queue: rabbitMqConfig.Value.QueueName,
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false);
            }
            catch
            {
               
            }
        }
        public void SendMessage(Email email)
        {

            if (channel == null || channel.IsClosed) return;

            email.EmailFrom = _emailSettings.FromAddress;
            email.NameFrom = _emailSettings.FromName;

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;            
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(email));            
            channel.BasicPublish(exchange: string.Empty,
                routingKey: queueName,
                basicProperties: properties,
                body: body);
        }
        ~RabbitMqSender()
        {
            if (connection is not null)
            {
                channel.Close();
                connection.Close();
                channel.Dispose();
                connection.Dispose();
            }
        }
    }
}
