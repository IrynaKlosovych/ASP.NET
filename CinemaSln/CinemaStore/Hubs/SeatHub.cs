using Microsoft.AspNetCore.SignalR;

namespace CinemaStore.Hubs
{
    public class SeatHub : Hub
    {
        public async Task SeatAdded(int screeningId, string seat)
        {
            await Clients.Others.SendAsync("SeatAdded", screeningId, seat);
        }
        public async Task SeatRemoved(int screeningId, string seat)
        {
            await Clients.Others.SendAsync("SeatRemoved", screeningId, seat);
        }
    }
}
