using Microsoft.AspNetCore.SignalR;

namespace SignalRLab3
{
    public class ChatHub : Hub
    {
        public async Task SendMessageToUser(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveMessage", message);
        }

        public async Task SendMessageToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            var user = Context.UserIdentifier;
            if (user != null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, user);
            }
        }
        // Join a group
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        // Leave a group
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
