using Microsoft.AspNetCore.SignalR;

namespace Auto.WebSite.Hubs
{
    public class ZooHub : Hub
    {
        public async Task NotifyWebUsers(string user, string message)
        {
            await Clients.All.SendAsync("DisplayNotification", user, message);
        }
    }
}
