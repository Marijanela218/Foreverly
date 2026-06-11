using Microsoft.AspNetCore.SignalR;

namespace Foreverly.Hubs
{
    public class SeatingHub : Hub
    {
        public async Task SeatingUpdated()
        {
            await Clients.All.SendAsync("RefreshSeating");
        }
    }
}