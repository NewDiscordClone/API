using Application.Commands.Messages.RemoveReaction;
using Application.Exceptions;
using Application.Models;
using MongoDB.Driver;
using Tests.Common;

namespace Tests.Messages.Commands
{
    public class RemoveReactionsTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            var messageId = Ids.Message1;
            int reactionIndex = 0;

            SetAuthorizedUserId(Ids.UserBId);

            RemoveReactionRequest request = new()
            {
                MessageId = messageId,
                ReactionIndex = reactionIndex
            };
            RemoveReactionRequestHandler handler = new(Context, UserProvider);

            //Act
            Context.SetToken(CancellationToken);
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
            var messageId = Ids.Message1;
            int reactionIndex = 0;

            SetAuthorizedUserId(Ids.UserAId);

            RemoveReactionRequest request = new()
            {
                MessageId = messageId,
                ReactionIndex = reactionIndex
            };
            RemoveReactionRequestHandler handler = new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(async ()
                => await handler.Handle(request, CancellationToken));
        }
    }
}
