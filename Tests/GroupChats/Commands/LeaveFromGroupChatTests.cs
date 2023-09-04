﻿using Application.Commands.GroupChats.LeaveFromGroupChat;
using Application.Common.Exceptions;
using Application.Models;
using MongoDB.Driver;
using Tests.Common;

namespace Tests.GroupChats.Commands
{
    public class LeaveFromGroupChatTests : TestBase
    {
        [Fact]
        public async Task Success_Owner()
        {
            //Arrange
            CreateDatabase();
            var chatId = Ids.GroupChat7;
            int userId = Ids.UserBId;

            SetAuthorizedUserId(userId);

            LeaveFromGroupChatRequest request = new()
            {
                ChatId = chatId,
            };

            LeaveFromGroupChatRequestHandler handler = new(Context, UserProvider);

            //Act
            Context.SetToken(CancellationToken);
            await handler.Handle(request, CancellationToken);
            PersonalChat chat = await Context.PersonalChats.FindAsync(chatId);

            //Assert
            Assert.DoesNotContain(chat.Users, user => user.Id == userId);
            var groupChat = chat as GroupChat;
            Assert.NotNull(groupChat);
            Assert.NotEqual(userId, groupChat.OwnerId);
        }

        [Fact]
        public async Task Success_LastUser()
        {
            //Arrange
            CreateDatabase();
            var chatId = Ids.GroupChat4;
            int userId = Ids.UserAId;

            SetAuthorizedUserId(userId);

            LeaveFromGroupChatRequest request = new()
            {
                ChatId = chatId,
            };

            LeaveFromGroupChatRequestHandler handler = new(Context, UserProvider);

            //Act
            Context.SetToken(CancellationToken);
            await handler.Handle(request, CancellationToken);

            //Assert
            await Assert.ThrowsAsync<EntityNotFoundException>( 
                async () => await Context.PersonalChats.FindAsync(chatId));
        }
        [Fact]
        public async Task Fail_NoSuchUser()
        {
            //Arrange
            CreateDatabase();
            var chatId = Ids.GroupChat4;
            int userId = Ids.UserBId;

            SetAuthorizedUserId(userId);

            LeaveFromGroupChatRequest request = new()
            {
                ChatId = chatId,
            };

            LeaveFromGroupChatRequestHandler handler = new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoSuchUserException>(async ()
                => await handler.Handle(request, CancellationToken));
        }
    }
}