using Application.Interfaces;
using Application.Messages.AddMessageRequest;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Application.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IAppDbContext _dbContext;
    private readonly ILogger<ChatHub> _logger;
    private readonly IMediator _mediator;

    private event Func<Message, PrivateChat, Task> OnMessageReceived;
    public ChatHub(IAppDbContext dbContext, IMediator mediator, ILogger<ChatHub> logger)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _logger = logger;

        OnMessageReceived += SendMessageToPrivateChat;
    }

    private static readonly Dictionary<int, HashSet<string>> _userConnections = new();
    private int UserId => int.Parse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? throw new Exception("User not authenticated"));
    public override async Task OnConnectedAsync()
    {
        if (!_userConnections.ContainsKey(UserId))
        {
            _userConnections[UserId] = new HashSet<string>();
        }

        _userConnections[UserId].Add(Context.ConnectionId);
        _logger.LogInformation($"User {UserId} connected");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (_userConnections.ContainsKey(UserId))
        {
            _userConnections[UserId].Remove(Context.ConnectionId);
            if (_userConnections[UserId].Count == 0)
            {
                _userConnections.Remove(UserId);
                _logger.LogInformation($"User {UserId} disconnected");
            }
        }

        await base.OnDisconnectedAsync(exception);
    }
    public async Task AddMessage(string text, int chatId)
    {
        if (!_userConnections.ContainsKey(UserId))
        {
            return;
        }
        AddMessageRequest messageRequest = new()
        {
            Text = text,
            ChatId = chatId,
            UserId = UserId
        };
        try
        {
            Message message = await _mediator.Send(messageRequest);
            _logger.LogInformation($"User {UserId} sent message to chat {chatId}");

            if (message.Chat is PrivateChat chat)
            {
                OnMessageReceived?.Invoke(message, chat);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error while user {UserId} tries send message to chat {chatId}");
        }
    }

    private async Task SendMessageToPrivateChat(Message message, PrivateChat chat)
    {
        IEnumerable<string> connectedUsers = chat.Users
            .Where(user => _userConnections.ContainsKey(user.Id))
            .SelectMany(user => _userConnections[user.Id]);

        foreach (string connectionId in connectedUsers)
        {
            await Clients.Client(connectionId).SendAsync("MessageAdded", message);
        }
    }
}