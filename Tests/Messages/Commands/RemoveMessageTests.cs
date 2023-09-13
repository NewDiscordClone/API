using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Messages.Commands.RemoveMessage;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Messages.Commands
{
    public class RemoveMessageTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();

            string messageId = Ids.Message1;

            SetAuthorizedUserId(Ids.UserAId);

            RemoveMessageRequest request = new()
            {
                MessageId = messageId,
            };
            RemoveMessageRequestHandler handler = new(Context, UserProvider);

            //Act

            await handler.Handle(request, CancellationToken);

            //Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(async () => await Context.Messages.FindAsync(messageId));
        }

        [Fact]
        public async Task Fail_NoPermissions()
        {
            //Arrange
            CreateDatabase();

            string messageId = Ids.Message1;

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
