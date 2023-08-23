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
            CreateDatabase();
            //Arrange
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
            await handler.Handle(request, CancellationToken);
            Message? message = Context.Messages.Find(Context.GetIdFilter<Message>(messageId)).FirstOrDefault();
            
            //Assert
            Assert.NotNull(message);
            Assert.Single(message.Reactions);
        }

        [Fact]
        public async Task Fail_NoPermissions()
        {
            CreateDatabase();
            //Arrange
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
