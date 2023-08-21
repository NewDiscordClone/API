using Application.Commands.Messages.EditMessage;
using Application.Exceptions;
using Application.Models;
using Tests.Common;

namespace Tests.Messages.Commands
{
    public class EditMessageTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            int messageId = 1;
            string newText = "Edited text https://img-s-msn-com.akamaized.net/tenant/amp/entityid/AA12gqVZ.img?w=800&h=415&q=60&m=2&f=jpg";

            SetAuthorizedUserId(TestDbContextFactory.UserAId);

            EditMessageRequest request = new()
            {
                MessageId = messageId,
                NewText = newText,
            };
            EditMessageRequestHandler handler = new(Context, UserProvider);

            //Act
            Message result = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.Equal(newText, result.Text);
            Assert.Equal(AttachmentType.UrlImage, result.Attachments[0].Type);
        }

        [Fact]
        public async Task Fail_NoPermissions()
        {
            //Arrange
            int messageId = 1;
            string newText = "Edited text https://img-s-msn-com.akamaized.net/tenant/amp/entityid/AA12gqVZ.img?w=800&h=415&q=60&m=2&f=jpg";

            SetAuthorizedUserId(TestDbContextFactory.UserBId);

            EditMessageRequest request = new()
            {
                MessageId = messageId,
                NewText = newText,
            };
            EditMessageRequestHandler handler = new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(async ()
                => await handler.Handle(request, CancellationToken));
        }
    }
}
