﻿using MediatR;

namespace Application.Users.Commands.FriendRequest
{
    public record FriendRequestRequest : IRequest<string?>
    {
        /// <summary>
        /// The unique identifier of the user to send a friend request to.
        /// </summary>
        public Guid UserId { get; init; }
    }
}