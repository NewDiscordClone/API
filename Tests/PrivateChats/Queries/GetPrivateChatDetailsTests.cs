using Application.Models;
using Application.Providers;
using Application.Queries.GetPrivateChatDetails;
using Tests.Common;

namespace Tests.PrivateChats.Queries
{
    public class GetPrivateChatDetailsTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            var chatId = Ids.PrivateChat4;
            int userId = Ids.UserAId;
            
            SetAuthorizedUserId(userId);

            GetPrivateChatDetailsRequest request = new() { ChatId = chatId };
            GetPrivateChatDetailsRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            PrivateChat chat = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.NotNull(chat);
            Assert.Contains(chat.Users, user => user.Id == userId);
            Assert.Equal(chatId, chat.Id);
        }
    }
}
