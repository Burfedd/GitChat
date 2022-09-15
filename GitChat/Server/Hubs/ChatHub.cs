using Microsoft.AspNetCore.SignalR;

namespace GitChat.Server.Hubs
{
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            var name = Context.GetHttpContext().Request.Query["name"];
            return Clients.All.SendAsync("Send", $"{name} joined the chat");
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var name = Context.GetHttpContext().Request.Query["name"];
            return Clients.All.SendAsync("Send", $"{name} left the chat");
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);

        }

        public Task SendToGroup(string group, string name, string message)
        {
            return Clients.Group(group).SendAsync("ReceiveMessage", name, group, message);
        }


        public async Task JoinGroup(string groupName, string name)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{name} joined {groupName}");
        }

        public async Task LeaveGroup(string groupName, string name)
        {
            await Clients.Group(groupName).SendAsync("Send", $"{name} left {groupName}");

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
