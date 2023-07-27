using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs;

public class ChatHub : Hub
{
    public void AddMessage(string message)
    {
        // Call the MessageAdded method to update clients.
        Clients.All.SendAsync("MessageAdded", message);
    }
}