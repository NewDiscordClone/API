using Sparkle.Application.Messages.Commands.PinMessage;
using Sparkle.Application.Models;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Application.Messages.Commands
{
    public class PinMessageTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string messageId = Ids.Message1;

            SetAuthorizedUserId(Ids.UserAId);

            PinMessageCommand request = new()
            {
                MessageId = messageId
            };
            PinMessageCommandHandler handler = new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);
            Message result = await Context.Messages.FindAsync(messageId);

            //Assert

            Assert.True(result.IsPinned);
        }
    }
}