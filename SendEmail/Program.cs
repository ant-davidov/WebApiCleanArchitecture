using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendEmail.Application;
using SendEmail.Application.Interfaces;
using SendEmail.Infrastructure;
using SendEmail.Infrastructure.Models;
using System;
using System.Runtime.CompilerServices;

var services = new ServiceCollection()
            .AddTransient<IEmailSender, EmailSender>()
            .AddTransient<IMessageService, MessageService>()
            .BuildServiceProvider();

var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("Parameters.json", optional: true, reloadOnChange: true)
                                .AddEnvironmentVariables();

var isDocker = Environment.GetEnvironmentVariable("RABBIT_CONFIG");
var configuration = builder.Build();
var rabbitMqConfig = new RabbitMqConfig();
if (isDocker == null)
    configuration.GetSection("RabbitConfig").Bind(rabbitMqConfig);
else
    configuration.GetSection("RABBIT_CONFIG").Bind(rabbitMqConfig);




var messageService = services.GetRequiredService<IMessageService>();
var rabbitMQSubscriber = new RabbitMQSubscriber(rabbitMqConfig, messageService.ProcessMessage);
rabbitMQSubscriber.Subscribe();



Console.WriteLine("Ожидание сообщений. Нажмите любую клавишу для выхода.");
Console.ReadKey();




