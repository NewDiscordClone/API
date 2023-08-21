using Application.Commands.Messages.RemoveReaction;
using Application.Exceptions;
using Tests.Common;

namespace Tests.Messages.Commands
{
    public class RemoveReactionsTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            int reactionId = 1;

            SetAuthorizedUserId(TestDbContextFactory.UserBId);

            RemoveReactionRequest request = new()
            {
                ReactionId = reactionId
            };
            RemoveReactionRequestHandler handler = new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);

            //Assert
            Assert.Null(Context.Reactions.Find(reactionId));
        }

        [Fact]
        public async Task Fail_NoPermissions()
        {
            //Arrange
            int reactionId = 1;

            SetAuthorizedUserId(TestDbContextFactory.UserAId);

            RemoveReactionRequest request = new()
            {
                ReactionId = reactionId
            };
            RemoveReactionRequestHandler handler = new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(async ()
                => await handler.Handle(request, CancellationToken));
        }
    }
}
