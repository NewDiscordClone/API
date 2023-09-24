﻿using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.GroupChats.Commands.AddMemberToGroupChat;
using Sparkle.Application.Models;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Application.GroupChats.Commands
{
    public class AddMemberToGroupChatTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();

            Guid newMemberId = Ids.UserAId;
            string chatId = Ids.GroupChat5;
            int oldUsersCount = 2;

            SetAuthorizedUserId(Ids.UserBId);

            AddMemberToGroupChatCommand request = new()
            {
                ChatId = chatId,
                NewMemberId = newMemberId
            };

            AddMemberToGroupChatCommandHandler handler = new(Context, UserProvider, Mapper);

            //Act

            await handler.Handle(request, CancellationToken);
            PersonalChat chat = await Context.PersonalChats.FindAsync(chatId);

            //Assert
            Assert.Equal(oldUsersCount + 1, chat.Profiles.Count);
            Assert.Contains(chat.Profiles, profile => profile.UserId == newMemberId);
        }
        [Fact]
        public async Task UserAlreadyExists_Fail()
        {
            //Arrange
            CreateDatabase();
            Guid newMemberId = Ids.UserCId;
            string chatId = Ids.GroupChat5;

            SetAuthorizedUserId(Ids.UserBId);

            AddMemberToGroupChatCommand request = new()
            {
                ChatId = chatId,
                NewMemberId = newMemberId
            };

            AddMemberToGroupChatCommandHandler handler =
                new(Context, UserProvider, Mapper);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(async ()
                => await handler.Handle(request, CancellationToken));
        }
    }
}