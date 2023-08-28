using Application.Commands.Messages.RemoveMessage;
using Application.Exceptions;
using Application.Models;
using MongoDB.Driver;
using Tests.Common;

namespace Tests.Messages.Commands
{
    public class RemoveMessageTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();

            var messageId = Ids.Message1;

            SetAuthorizedUserId(Ids.UserAId);

            RemoveMessageRequest request = new()
            {
                MessageId = messageId,
            };
            RemoveMessageRequestHandler handler = new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);

            //Assert
            Assert.Null(Context.Messages.Find(Context.GetIdFilter<Message>(messageId)).FirstOrDefault());
        }

        [Fact]
        public async Task Fail_NoPermissions()
        {
            //Arrange
            CreateDatabase();

            var messageId = Ids.Message1;

            SetAuthorizedUserId(Ids.UserBId);

            RemoveMessageRequest request = new()
            {
                MessageId = messageId,
            };
            RemoveMessageRequestHandler handler = new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(async ()
                => await handler.Handle(request, CancellationToken));
        }
    }
}
