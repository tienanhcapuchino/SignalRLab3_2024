using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRLab3.DataAccess;
using SignalRLab3.Entities;

namespace SignalRLab3
{
    public class ChatHub : Hub
    {
        private SignalRLab3DbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;


        public List<User> Users { get; set; } = new();
        public ChatHub(SignalRLab3DbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _contextAccessor = httpContextAccessor;
        }

        public async Task SendBroadCast(string message)
        {
            await Clients.All.SendAsync("ReceivedMess", "someone", message);
        }

        public async Task SendMess(string user, string message)
        {
            UserList.UserConnections.TryGetValue(user, out var connectionId);

            if (string.IsNullOrEmpty(connectionId)) return;

            await Clients.Client(connectionId).SendAsync("ReceivedMess", user, message);
        }

        public async Task SendMessageToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceivedMessGroup", message);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = _contextAccessor.HttpContext;
            var userEmail = httpContext.Session.GetString("user");
            var userId = httpContext.Session.GetString("userId");

            if (!string.IsNullOrEmpty(userEmail))
            {
                UserList.AddUser(userEmail, Context.ConnectionId);
                var groupId = await _context.Users.Where(x => x.Id == new Guid(userId)).Select(x => x.GroupId).FirstOrDefaultAsync();
                var group = await _context.Groups.Where(g => g.Id == groupId).Include(x => x.Users).FirstOrDefaultAsync();

                if (group != null)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, group.Name);
                }
            }
            await base.OnConnectedAsync();
        }
    }
}
