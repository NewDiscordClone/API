﻿using Sparkle.Application.Messages.Queries.GetPinnedMessages;
using Sparkle.Application.Models;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Messages.Queries
{
    public class GetPinnedMessagesTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string chatId = Ids.GroupChat3;

            SetAuthorizedUserId(Ids.UserAId);

            GetPinnedMessagesQuery request = new()
            {
                ChatId = chatId
            };
            GetPinnedMessagesQueryHandler handler = new(Context, UserProvider, Mapper);

            //Act

            List<Message> result = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.True(result.All(message => Context.Messages.FindAsync(message.Id).Result.IsPinned));
        }
    }
}