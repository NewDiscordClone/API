using Application.Commands.Messages.RemoveAllReactions;
using Application.Models;
using Tests.Common;

namespace Tests.Messages.Commands
{
    public class RemoveAllReactionsTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string messageId = Ids.Message1;

            SetAuthorizedUserId(Ids.UserBId);

            RemoveAllReactionsRequest request = new()
            {
                MessageId = messageId
            };
            RemoveAllReactionsRequestHandler handler = new(Context, UserProvider);

            //Act

            await handler.Handle(request, CancellationToken);
            Message message = await Context.Messages.FindAsync(messageId);

            //Assert
            Assert.Empty(message.Reactions);
        }
    }
}