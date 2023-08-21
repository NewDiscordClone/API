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
            int attachmentId = 1;

            SetAuthorizedUserId(TestDbContextFactory.UserBId);

            RemoveAttachmentRequest request = new()
            {
                AttachmentId = attachmentId
            };

            RemoveAttachmentRequestHandler handler = new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);
            Attachment? attachment = Context.Attachments.Find(attachmentId);

            //Assert
            Assert.Null(attachment);
        }
    }
}
