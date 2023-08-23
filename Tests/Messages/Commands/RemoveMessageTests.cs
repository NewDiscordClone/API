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

            var messageId = TestDbContextFactory.Message1;

            SetAuthorizedUserId(TestDbContextFactory.UserAId);

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

            var messageId = TestDbContextFactory.Message1;

            SetAuthorizedUserId(TestDbContextFactory.UserBId);

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
