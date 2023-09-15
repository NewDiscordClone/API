using Sparkle.Application.Messages.Commands.UnpinMessage;
using Sparkle.Application.Models;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Messages.Commands
{
    public class UnpinMessageTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string messageId = Ids.Message2;

            SetAuthorizedUserId(Ids.UserAId);

            UnpinMessageCommand request = new()
            {
                MessageId = messageId,
            };
            UnpinMessageCommandHandler handler = new(Context, UserProvider);

            //Act
            Message result = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.False(result.IsPinned);
        }
    }
}
