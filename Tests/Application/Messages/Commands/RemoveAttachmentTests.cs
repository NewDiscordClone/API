using Sparkle.Application.Messages.Commands.RemoveAttachment;
using Sparkle.Application.Models;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Application.Messages.Commands
{
    public class RemoveAttachmentTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string messageId = Ids.Message2;
            int attachmentIndex = 0;

            SetAuthorizedUserId(Ids.UserBId);

            RemoveAttachmentCommand request = new()
            {
                MessageId = messageId,
                AttachmentIndex = attachmentIndex
            };

            RemoveAttachmentCommandHandler handler = new(Context, UserProvider);

            //Act

            await handler.Handle(request, CancellationToken);
            Message? message = await Context.Messages.FindAsync(messageId);

            //Assert
            Assert.Empty(message.Attachments);
        }
    }
}