using Application.Commands.Messages.RemoveAttachment;
using Application.Models;
using MongoDB.Driver;
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
            var messageId = Ids.Message2;
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
            Message? message = Context.Messages
                .Find(Context.GetIdFilter<Message>(messageId))
                .FirstOrDefault();

            //Assert

            Assert.NotNull(message);
            Assert.Empty(message.Attachments);
        }
    }
}