﻿using Application.Commands.GroupChats.AddMemberToGroupChat;
using Application.Common.Exceptions;
using Application.Models;
using MongoDB.Driver;
using Tests.Common;

namespace Tests.GroupChats.Commands
{
    public class AddMemberToGroupChatTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();

            int newMemberId = Ids.UserAId;
            var chatId = Ids.GroupChat5;
            int oldUsersCount = 2;

            SetAuthorizedUserId(Ids.UserBId);

            AddMemberToGroupChatRequest request = new()
            {
                ChatId = chatId,
                NewMemberId = newMemberId
            };

            AddMemberToGroupChatRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            Context.SetToken(CancellationToken);
            await handler.Handle(request, CancellationToken);
            PersonalChat chat =await Context.PersonalChats.FindAsync(chatId);

            //Assert
            Assert.Equal(oldUsersCount + 1, chat.Users.Count);
            Assert.Contains(chat.Users, user => user.Id == newMemberId);
        }
        [Fact]
        public async Task UserAlreadyExists_Fail()
        {
            //Arrange
            CreateDatabase();
            int newMemberId = Ids.UserCId;
            var chatId = Ids.GroupChat5;

            SetAuthorizedUserId(Ids.UserBId);

            AddMemberToGroupChatRequest request = new()
            {
                ChatId = chatId,
                NewMemberId = newMemberId
            };

            AddMemberToGroupChatRequestHandler handler =
                new(Context, UserProvider, Mapper);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(async ()
                => await handler.Handle(request, CancellationToken));
        }
    }
}