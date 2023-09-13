using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.HubClients.Connections.Connect;
using Sparkle.Application.HubClients.Connections.Disconect;

namespace Sparkle.WebApi.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly ILogger<ChatHub> _logger;
    private readonly IMediator _mediator;
    private readonly IAuthorizedUserProvider _userProvider;

    public ChatHub(IAuthorizedUserProvider userProvider, IMediator mediator, ILogger<ChatHub> logger)
    {
        _mediator = mediator;
        _logger = logger;
        _userProvider = userProvider;
    }
    private Guid UserId => _userProvider.GetUserId();
    public override async Task OnConnectedAsync()
    {
        await _mediator.Send(new ConnectRequest() { ConnectionId = Context.ConnectionId });
        _logger.LogInformation($"User {UserId} connected");
        await base.OnConnectedAsync();
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _mediator.Send(new DisconnectRequest() { ConnectionId = Context.ConnectionId });
        _logger.LogInformation($"User {UserId} disconnected");
        await base.OnDisconnectedAsync(exception);
    }
}