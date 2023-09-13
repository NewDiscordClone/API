﻿using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;
using MongoDB.Driver;

namespace Application.Messages.Commands.AddReaction
{
    public class AddReactionRequestHandler : RequestHandlerBase, IRequestHandler<AddReactionRequest>
    {
        public async Task Handle(AddReactionRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Message message = await Context.Messages.FindAsync(request.MessageId);
            Chat chat = await Context.Chats.FindAsync(message.ChatId);

            if (!chat.Users.Any(u => u == UserId))
                throw new NoPermissionsException("You are not a member of the Chat");

            Reaction reaction = new()
            {
                User = UserId,
                Emoji = request.Emoji,
            };

            await Context.Messages.UpdateAsync(message);
        }

        public AddReactionRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) :
            base(context, userProvider, mapper)
        {
        }
    }
}