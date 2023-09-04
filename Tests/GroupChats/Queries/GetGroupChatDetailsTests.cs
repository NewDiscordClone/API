using Application.Models;
using Application.Interfaces;
using Application.Queries.GetGroupChatDetails;
using Tests.Common;

namespace Tests.GroupChats.Queries
{
    public class GetGroupChatDetailsTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            var chatId = Ids.GroupChat4;
            Guid userId = Ids.UserAId;
            
            SetAuthorizedUserId(userId);

            GetGroupChatDetailsRequest request = new() { ChatId = chatId };
            GetGroupChatDetailsRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            PersonalChat chat = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.NotNull(chat);
            Assert.Contains(chat.Users, user => user.Id == userId);
            Assert.Equal(chatId, chat.Id);
        }
    }
}
