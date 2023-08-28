using Application.Commands.Messages.AddMessage;
using Application.Interfaces;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using MongoDB.Driver;
using WebApi.Models;

namespace Application.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly ILogger<ChatHub> _logger;
    private readonly IMediator _mediator;
    private readonly IAuthorizedUserProvider _userProvider;
    private readonly IAppDbContext _context;

    private event Action<Message, Chat> OnChatMessageReceived;

    public ChatHub(IAppDbContext appDbContext, IAuthorizedUserProvider userProvider, IMediator mediator, ILogger<ChatHub> logger)
    {
        _mediator = mediator;
        _logger = logger;
        _userProvider = userProvider;
        _context = appDbContext;

        OnChatMessageReceived += async (msg, chat) => await SendMessageToChat(msg, chat);
    }

    private static readonly Dictionary<int, HashSet<string>> _userConnections = new();

    private int UserId => _userProvider.GetUserId();

    // int.Parse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
    // ?? throw new Exception("User not authenticated"));
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

    public async Task AddMessage(AddMessageRequest messageRequest)
    {
        if (!_userConnections.ContainsKey(UserId))
        {
            return;
        }

        try
        {
            Message message = await _mediator.Send(messageRequest);
            _logger.LogInformation($"User {UserId} sent message to chat {messageRequest.ChatId}");

            OnChatMessageReceived?.Invoke(message, await _context.Chats.FindAsync(message.ChatId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error while user {UserId} tries send message to chat {messageRequest.ChatId}");
        }
    }

    private async Task SendMessageToChat(Message message, Chat chat)
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