using Application.Commands.Messages.AddMessage;
using Application.Models;
using MongoDB.Driver;
using Tests.Common;

namespace Tests.Messages.Commands
{
    public class AddMessageTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string messageText = "Test message";

            SetAuthorizedUserId(Ids.UserAId);

            AddMessageRequest request = new()
            {
                Attachments = new()
                {
                    new Attachment
                    {
                        IsInText = false,
                        Path =
                            "https://img-s-msn-com.akamaized.net/tenant/amp/entityid/AA12gqVZ.img?w=800&h=415&q=60&m=2&f=jpg"
                    }
                },
                ChatId = Ids.PrivateChat3,
                Text = messageText
            };

            AddMessageRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            Message result = await handler.Handle(request, CancellationToken);

            //Assert

            Assert.Contains(await Context.Messages.FilterAsync(_ => true), e => e.Id == result.Id);
            Assert.NotEmpty(result.Attachments);
            Assert.Equal(messageText, result.Text);
        }

        [Fact]
        public async Task Success_AttachmentInText()
        {
            //Arrange
            CreateDatabase();
            string messageText =
                "https://img-s-msn-com.akamaized.net/tenant/amp/entityid/AA12gqVZ.img?w=800&h=415&q=60&m=2&f=jpg";

            SetAuthorizedUserId(Ids.UserAId);

            AddMessageRequest request = new()
            {
                ChatId = Ids.PrivateChat3,
                Text = messageText
            };

            AddMessageRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            Context.SetToken(CancellationToken);
            Message result = await handler.Handle(request, CancellationToken);

            //Assert

            Assert.Contains(await Context.Messages.FilterAsync(_ => true), e => e.Id == result.Id);
            Assert.NotEmpty(result.Attachments);
            Assert.True(result.Attachments[0].IsInText);
            Assert.Equal(messageText, result.Text);
        }
    }
}