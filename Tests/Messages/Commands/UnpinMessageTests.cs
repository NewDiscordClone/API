﻿using Application.Messages.Commands.UnpinMessage;
using Application.Models;
using Tests.Common;

namespace Tests.Messages.Commands
{
    public class UnpinMessageTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string messageId = Ids.Message2;

            SetAuthorizedUserId(Ids.UserAId);

            UnpinMessageRequest request = new()
            {
                MessageId = messageId,
            };
            UnpinMessageRequestHandler handler = new(Context, UserProvider);

            //Act
            Message result = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.False(result.IsPinned);
        }
    }
}
