using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Core.Service.Hubs
{
    public class BookHub : Hub
    {
        public async Task NotifyBookCreated(string bookTitle)
        {
            await Clients.All.SendAsync("BookCreated", bookTitle);
        }
    }
}
