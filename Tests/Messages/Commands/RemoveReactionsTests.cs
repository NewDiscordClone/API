using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Messages.Commands.RemoveReaction;
using Sparkle.Application.Models;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Messages.Commands
{
    public class RemoveReactionsTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string messageId = Ids.Message1;
            int reactionIndex = 0;

            SetAuthorizedUserId(Ids.UserBId);

            RemoveReactionCommand request = new()
            {
                MessageId = messageId,
                ReactionIndex = reactionIndex
            };
            RemoveReactionCommandHandler handler = new(Context, UserProvider);

            //Act

            await handler.Handle(request, CancellationToken);
            Message message = await Context.Messages.FindAsync(messageId);

            //Assert
            Assert.Single(message.Reactions);
        }

        [Fact]
        public async Task Fail_NoPermissions()
        {
            //Arrange
            CreateDatabase();
            string messageId = Ids.Message1;
            int reactionIndex = 0;

            SetAuthorizedUserId(Ids.UserAId);

            RemoveReactionCommand request = new()
            {
                MessageId = messageId,
                ReactionIndex = reactionIndex
            };
            RemoveReactionCommandHandler handler = new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(async ()
                => await handler.Handle(request, CancellationToken));
        }
    }
}
