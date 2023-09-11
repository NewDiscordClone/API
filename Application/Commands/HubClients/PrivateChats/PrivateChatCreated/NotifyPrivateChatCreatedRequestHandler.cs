﻿using Application.Application;
using Application.Interfaces;
using Application.Models;
using Application.Models.LookUps;
using Application.Providers;
using AutoMapper;
using MediatR;

namespace Application.Commands.HubClients.PrivateChats.PrivateChatCreated
{
    public class NotifyPrivateChatCreatedRequestHandler : HubRequestHandlerBase,
        IRequestHandler<NotifyPrivateChatCreatedRequest>
    {
        public NotifyPrivateChatCreatedRequestHandler(
            IHubContextProvider hubContextProvider,
            IAppDbContext context,
            IAuthorizedUserProvider userProvider,
            IMapper mapper)
            : base(hubContextProvider, context, userProvider, mapper)
        {
        }

        public async Task Handle(NotifyPrivateChatCreatedRequest request, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            Chat chat = await Context.PersonalChats.FindAsync(request.ChatId);
            foreach (var user in chat.Users)
            {
                await SendToUser(chat, user);
            }
        }

        private async Task SendToUser(Chat chat, Guid userId)
        {
            User user = await Context.SqlUsers.FindAsync(userId);
            PrivateChatLookUp lookUp = null;
            switch(chat)
            {
                case GroupChat gChat:
                    lookUp = Mapper.Map<PrivateChatLookUp>(gChat);
                    break; 
                case PersonalChat pChat:
                    User other = await Context.SqlUsers.FindAsync(chat.Users.First(u => u != userId));
                    lookUp = new PrivateChatLookUp(pChat, Mapper.Map<UserLookUp>(other));
                    break;
                default:
                    throw new ArgumentException("the given chat is not an private chat");
            };
            await SendAsync(ClientMethods.PrivateChatCreated, lookUp, GetConnections(user));
        }
    }
}