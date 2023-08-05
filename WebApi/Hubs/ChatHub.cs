using System.Security.Claims;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebApi.Models;

namespace WebApi.Hubs;

[Authorize]
public class ChatHub : Hub
{
    
    private readonly AppDbContext _dbContext;
    public ChatHub(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    private static readonly Dictionary<string, HashSet<string>> _userConnections = new ();
    private string? _userId = null;
    private string UserId => _userId ?? (_userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    public override async Task OnConnectedAsync()
    {
        if (!_userConnections.ContainsKey(UserId))
        {
            _userConnections[UserId] = new HashSet<string>();
        }

        _userConnections[UserId].Add(Context.ConnectionId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        if (_userConnections.ContainsKey(UserId))
        {
            _userConnections[UserId].Remove(Context.ConnectionId);
            if (_userConnections[UserId].Count == 0)
            {
                _userConnections.Remove(UserId);
            }
        }

        await base.OnDisconnectedAsync(exception);
    }
    public void AddMessage(string text, int chatId)
    {
        if (!_userConnections.ContainsKey(UserId))
        {
            // User is not connected, handle accordingly
            return;
        }

        var chat = _dbContext.PrivateChats.Find(chatId);

        if (chat == null)
        {
            // Chat not found, handle accordingly
            return;
        }

        var message = new Message
        {
            Text = text,
            SendTime = DateTime.UtcNow,
            User = _dbContext.Users.Find(UserId),
            Chat = chat
        };

        _dbContext.Messages.Add(message);
        _dbContext.SaveChanges();

        foreach (string? connectionId in chat.Users
                     .Where(user => _userConnections.ContainsKey(user.Id))
                     .SelectMany(user => _userConnections[user.Id]))
        {
            Clients.Client(connectionId).SendAsync("MessageAdded", message);
        }
    }
}