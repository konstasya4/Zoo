using EasyNetQ;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using Auto.Messages;

namespace Auto.AuditLog
{
    internal class Program
    {
        private static readonly IConfigurationRoot config = ReadConfiguration();

        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Auto.AuditLog");
            var amqp = config.GetConnectionString("AutoRabbitMQ");
            using var bus = RabbitHutch.CreateBus(amqp);
            Console.WriteLine("Connected to bus! Listening for newProductMessages");
            var subscriberId = $"Auto.AuditLog@{Environment.MachineName}";
            await bus.PubSub.SubscribeAsync<NewProductMessage>(subscriberId, HandleNewProductMessage);
            Console.ReadLine();
        }

        private static void HandleNewProductMessage(NewProductMessage npm)
        {
            var csvRow = $"{npm.Serial},{npm.CategoryTitle},{npm.AnimalTitle},{npm.Price}";
            Console.WriteLine(csvRow);
        }

        private static IConfigurationRoot ReadConfiguration()
        {
            var basePath = Directory.GetParent(AppContext.BaseDirectory).FullName;
            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }
    }
}