using Sparkle.Application.Messages.Commands.RemoveAllReactions;
using Sparkle.Application.Models;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Application.Messages.Commands
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

            RemoveAllReactionsCommand request = new()
            {
                MessageId = messageId
            };
            RemoveAllReactionsCommandHandler handler = new(Context, UserProvider);

            //Act

            await handler.Handle(request, CancellationToken);
            Message message = await Context.Messages.FindAsync(messageId);

            //Assert
            Assert.Empty(message.Reactions);
        }
    }
}