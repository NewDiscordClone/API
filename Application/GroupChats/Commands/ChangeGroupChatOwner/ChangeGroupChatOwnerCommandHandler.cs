﻿using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.GroupChats.Commands.ChangeGroupChatOwner
{
    public class ChangeGroupChatOwnerCommandHandler : RequestHandlerBase, IRequestHandler<ChangeGroupChatOwnerCommand>
    {
        public async Task Handle(ChangeGroupChatOwnerCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat pchat = await Context.GroupChats.FindAsync(command.ChatId);
            if (pchat is not GroupChat chat)
                throw new Exception("This is not group chat");

            //TODO Удалить клейм из старого владельца и добавить в нового

            chat.OwnerId = command.ProfileId;
            await Context.GroupChats.UpdateAsync(chat);
        }

        public ChangeGroupChatOwnerCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}