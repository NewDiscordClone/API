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
            string messageText = "Test message";

            SetAuthorizedUserId(TestDbContextFactory.UserAId);

            AddMessageRequest request = new()
            {
                Attachments = new()
                {
                    new Attachment
                    {
                        Type = AttachmentType.UrlImage,
                        Path =
                            "https://img-s-msn-com.akamaized.net/tenant/amp/entityid/AA12gqVZ.img?w=800&h=415&q=60&m=2&f=jpg"
                    }
                },
                ChatId = TestDbContextFactory.PrivateChat3,
                Text = messageText
            };

            AddMessageRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            Message result = await handler.Handle(request, CancellationToken);

            //Assert

            Assert.Contains(Context.Messages.Find(Builders<Message>.Filter.Empty).ToList(), e => e.Id == result.Id);
            Assert.NotEmpty(result.Attachments);
            Assert.Equal(messageText, result.Text);
        }

        [Fact]
        public async Task Success_AttachmentInText()
        {
            //Arrange
            string messageText =
                "https://img-s-msn-com.akamaized.net/tenant/amp/entityid/AA12gqVZ.img?w=800&h=415&q=60&m=2&f=jpg";

            SetAuthorizedUserId(TestDbContextFactory.UserAId);

            AddMessageRequest request = new()
            {
                ChatId = TestDbContextFactory.PrivateChat3,
                Text = messageText
            };

            AddMessageRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            Message result = await handler.Handle(request, CancellationToken);

            //Assert

            Assert.Contains(Context.Messages.Find(Builders<Message>.Filter.Empty).ToList(), e => e.Id == result.Id);
            Assert.NotEmpty(result.Attachments);
            Assert.Equal(AttachmentType.UrlImage, result.Attachments[0].Type);
            Assert.Equal(messageText, result.Text);
        }
    }
}