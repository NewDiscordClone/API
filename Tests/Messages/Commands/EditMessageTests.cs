using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Messages.Commands.EditMessage;
using Sparkle.Application.Models;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Messages.Commands
{
    public class EditMessageTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string messageId = Ids.Message1;
            string newText =
                "Edited text https://img-s-msn-com.akamaized.net/tenant/amp/entityid/AA12gqVZ.img?w=800&h=415&q=60&m=2&f=jpg";

            SetAuthorizedUserId(Ids.UserAId);

            EditMessageCommand request = new()
            {
                MessageId = messageId,
                NewText = newText,
            };
            EditMessageCommandHandler handler = new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);
            Message result = await Context.Messages.FindAsync(messageId);

            //Assert

            Assert.Equal(newText, result.Text);
            Assert.True(result.Attachments[0].IsInText);
        }

        [Fact]
        public async Task Fail_NoPermissions()
        {
            //Arrange
            CreateDatabase();
            string messageId = Ids.Message1;
            string newText =
                "Edited text https://img-s-msn-com.akamaized.net/tenant/amp/entityid/AA12gqVZ.img?w=800&h=415&q=60&m=2&f=jpg";

            SetAuthorizedUserId(Ids.UserBId);

            EditMessageCommand request = new()
            {
                MessageId = messageId,
                NewText = newText,
            };
            EditMessageCommandHandler handler = new(Context, UserProvider);

            //Act
            //Assert

            await Assert.ThrowsAsync<NoPermissionsException>(async ()
                => await handler.Handle(request, CancellationToken));
        }
    }
}