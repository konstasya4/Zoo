//using Auto.NamesEngine;
//using Grpc.Net.Client;
//using var channel = GrpcChannel.ForAddress("https://localhost:7297");
//var grpcClient = new Namer.NamerClient(channel);
//Console.WriteLine("Ready! Press any key to send a gRPC request (or Ctrl-C to quit):");
//while (true)
//{
//    Console.ReadKey(true);
//    var request = new NameRequest
//    {
//        Serial = "14534",
//        Title = "hsfd",
//        Price = "1999"
//    };
//    var reply = grpcClient.GetName(request);
//    Console.WriteLine($"Name: {reply.Name}");
//}
using Auto.Messages;
using Auto.NamesEngine;
using EasyNetQ;
using Grpc.Net.Client;

namespace Autobarn.NamesClient
{
    class Program
    {
        private static Namer.NamerClient grpcClient;
        private static IBus bus;
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Auto.NamesClient");
            var amqp = "amqp://user:rabbitmq@localhost:5672";
            bus = RabbitHutch.CreateBus(amqp);
            Console.WriteLine("Connected to bus; Listening for newProductMessages");
            var grpcAddress = "https://localhost:7297";
            using var channel = GrpcChannel.ForAddress(grpcAddress);
            grpcClient = new Namer.NamerClient(channel);
            Console.WriteLine($"Connected to gRPC on {grpcAddress}!");
            var subscriberId = $"Auto.NamesClient@{Environment.MachineName}";
            await bus.PubSub.SubscribeAsync<NewProductMessage>(subscriberId, HandleNewProductMessage);
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }

        private static async Task HandleNewProductMessage(NewProductMessage message)
        {
            Console.WriteLine($"new product; {message.Serial}");
            var nameRequest = new NameRequest()
            {
                Title = message.Title,
                Animal = message.AnimalTitle,
                Category = message.CategoryTitle,
                Price = message.Price
            };
            var nameReply = await grpcClient.GetNameAsync(nameRequest);
            Console.WriteLine($"Product {message.Title} {nameReply.Name} {nameReply.WeightCode}");
            var newProductNameMessage = new NewProductNameMessage(message, nameReply.Name, nameReply.WeightCode);
            await bus.PubSub.PublishAsync(newProductNameMessage);
        }
    }
}

