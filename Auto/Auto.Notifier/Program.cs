using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using Auto.Messages;
using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using JsonSerializer = System.Text.Json.JsonSerializer;
namespace Auto.Notifier
{
    internal class Program
    {
        const string SIGNALR_HUB_URL = "https://localhost:7250/hub";
        private static HubConnection hub;
        static async Task Main(string[] args)
        {
            hub = new HubConnectionBuilder().WithUrl(SIGNALR_HUB_URL).Build();
            await hub.StartAsync();
            Console.WriteLine("Hub started!");
            Console.WriteLine("Press any key to send a message (Ctrl-C to quit)");
            var i = 0;
            while (true)
            {
                var input = Console.ReadLine();
                var message = $"Message #{i++} from Auto.Notifier {input}";
                await hub.SendAsync("NotifyWebUsers", "Auto.Notifier", message);
                Console.WriteLine($"Sent: {message}");
            }
        }
    }
}

