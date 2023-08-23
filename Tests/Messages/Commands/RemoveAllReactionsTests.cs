using Application.Commands.Messages.RemoveAllReactions;
using Application.Models;
using MongoDB.Driver;
using Tests.Common;

namespace Tests.Messages.Commands
{
    public class RemoveAllReactionsTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var messageId = TestDbContextFactory.Message1;

            SetAuthorizedUserId(TestDbContextFactory.UserBId);

            RemoveAllReactionsRequest request = new()
            {
                MessageId = messageId
            };
            RemoveAllReactionsRequestHandler handler = new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);
            Message? message = Context.Messages.Find(Context.GetIdFilter<Message>(messageId)).FirstOrDefault();

            //Assert

            Assert.NotNull(message);
            Assert.Empty(message.Reactions);
        }
    }
}