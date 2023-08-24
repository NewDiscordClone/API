using Application.Commands.Messages.RemoveMessage;
using Application.Common.Exceptions;
using Tests.Common;

namespace Tests.Messages.Commands
{
    public class RemoveMessageTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange

            int messageId = 1;

            SetAuthorizedUserId(TestDbContextFactory.UserAId);

            RemoveMessageRequest request = new()
            {
                MessageId = messageId,
            };
            RemoveMessageRequestHandler handler = new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);

            //Assert
            Assert.Null(Context.Messages.Find(messageId));
        }

        [Fact]
        public async Task Fail_NoPermissions()
        {
            //Arrange

            int messageId = 1;

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
