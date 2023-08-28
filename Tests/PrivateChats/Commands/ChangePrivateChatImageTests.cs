using Application.Commands.PrivateChats.ChangePrivateChatImage;
using Application.Models;
using MongoDB.Driver;
using Tests.Common;

namespace Tests.PrivateChats.Commands
{
    public class ChangePrivateChatImageTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            var chatId = Ids.PrivateChat4;
            const string newImage = "https://img-s-msn-com.akamaized.net/tenant/amp/entityid/AA12gqVZ.img?w=800&h=415&q=60&m=2&f=jpg";
            SetAuthorizedUserId(Ids.UserAId);
            
            ChangePrivateChatImageRequest request = new()
            {
                ChatId = chatId,
                NewImage = newImage
            };
            ChangePrivateChatImageRequestHandler handler = new(Context, UserProvider);
            
            //Act
            await handler.Handle(request, CancellationToken);
            PrivateChat? chat = Context.PrivateChats.Find(Context.GetIdFilter<PrivateChat>(chatId)).FirstOrDefault();
            
            //Assert
            Assert.NotNull(chat);
            Assert.Equal(newImage, chat.Image);
        }
    }
}
