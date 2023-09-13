﻿using Application.GroupChats.Queries.GetGroupChatDetails;
using Application.Models;
using Tests.Common;

namespace Tests.GroupChats.Queries
{
    public class GetGroupChatDetailsTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string chatId = Ids.GroupChat4;
            Guid userId = Ids.UserAId;

            SetAuthorizedUserId(userId);

            GetGroupChatDetailsRequest request = new() { ChatId = chatId };
            GetGroupChatDetailsRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            PersonalChat chat = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.NotNull(chat);
            Assert.Contains(chat.Users, user => user == userId);
            Assert.Equal(chatId, chat.Id);
        }
    }
}
