using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Hubs
{
    public class CommentHub : Hub
    {
        public static string HubUrl = "/commentHub";

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public async Task JoinProductGroup(string productId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, productId);
        }

        public async Task LeaveGroup(Guid productId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, productId.ToString());
        }
    }
}
