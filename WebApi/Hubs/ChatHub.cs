using Application.Commands.Messages.AddMessageRequest;
using Application.Interfaces;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Application.Providers;
using WebApi.Models;

namespace Application.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly ILogger<ChatHub> _logger;
    private readonly IMediator _mediator;
    private readonly IAuthorizedUserProvider _userProvider;

    private event Action<Message, PrivateChat> OnPrivateChatMessageReceived;
    private event Action<Message, Channel> OnChannelMessageReceived;
    public ChatHub(IAuthorizedUserProvider userProvider, IMediator mediator, ILogger<ChatHub> logger)
    {
        _mediator = mediator;
        _logger = logger;
        _userProvider = userProvider;
        
        OnPrivateChatMessageReceived += async (msg, chat) => await SendMessageToPrivateChat(msg, chat);
        OnChannelMessageReceived += async (msg, chat) => await SendMessageToChannel(msg, chat);
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

            if (message.Chat is PrivateChat chat)
            {
                OnPrivateChatMessageReceived?.Invoke(message, chat);
            }
            else if (message.Chat is Channel channel)
            {
                OnChannelMessageReceived?.Invoke(message, channel);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error while user {UserId} tries send message to chat {messageRequest.ChatId}");
        }
    }
    private async Task SendMessageToChannel(Message message, Channel channel)
    {
        IEnumerable<string> connectedUsers = channel.Server.ServerProfiles
            .Where(profile => _userConnections.ContainsKey(profile.User.Id))
            .SelectMany(profile => _userConnections[profile.User.Id]);

        foreach (string connectionId in connectedUsers)
        {
            await Clients.Client(connectionId).SendAsync("MessageAdded", message);
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