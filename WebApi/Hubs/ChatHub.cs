using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.HubClients.Connections.Connect;
using Sparkle.Application.HubClients.Connections.Disconnect;
using Sparkle.Application.HubClients.Users.UserUpdated;
using Sparkle.Domain;

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
        if (Context.User == null)
            throw new NoSuchUserException();
        _userProvider.SetUser(Context.User);

        User? user = await _mediator.Send(new AddUserConnectionCommand() { ConnectionId = Context.ConnectionId });
        _logger.LogDebug($"User {UserId} connected");

        if (user != null)
            await _mediator.Send(new NotifyUserUpdatedQuery() { UpdatedUser = user });

        await base.OnConnectedAsync();

    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.User == null)
            throw new NoSuchUserException();
        _userProvider.SetUser(Context.User);

        User? user = await _mediator.Send(new DeleteUserConnectionCommand() { ConnectionId = Context.ConnectionId });
        _logger.LogDebug($"User {UserId} disconnected");

        if (user != null)
            await _mediator.Send(new NotifyUserUpdatedQuery() { UpdatedUser = user });

        await base.OnDisconnectedAsync(exception);
    }
}