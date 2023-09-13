using Sparkle.Application.GroupChats.Commands.ChangeGroupChatImage;
using Sparkle.Application.Models;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.GroupChats.Commands
{
    public class ChangeGroupChatImageTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string chatId = Ids.GroupChat4;
            const string newImage = "https://img-s-msn-com.akamaized.net/tenant/amp/entityid/AA12gqVZ.img?w=800&h=415&q=60&m=2&f=jpg";
            SetAuthorizedUserId(Ids.UserAId);

            ChangeGroupChatImageRequest request = new()
            {
                ChatId = chatId,
                NewImage = newImage
            };
            ChangeGroupChatImageRequestHandler handler = new(Context, UserProvider);

            //Act

            await handler.Handle(request, CancellationToken);
            GroupChat chat = await Context.GroupChats.FindAsync(chatId);

            //Assert
            Assert.Equal(newImage, chat.Image);
        }
    }
}
