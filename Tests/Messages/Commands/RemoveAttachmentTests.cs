using Application.Commands.Messages.RemoveAttachment;
using Application.Models;
using Tests.Common;

namespace Tests.Messages.Commands
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

            RemoveAttachmentRequest request = new()
            {
                MessageId = messageId,
                AttachmentIndex = attachmentIndex
            };

            RemoveAttachmentRequestHandler handler = new(Context, UserProvider);

            //Act

            await handler.Handle(request, CancellationToken);
            Message? message = await Context.Messages.FindAsync(messageId);

            //Assert
            Assert.Empty(message.Attachments);
        }
    }
}