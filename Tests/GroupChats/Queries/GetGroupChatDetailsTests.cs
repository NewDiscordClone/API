using Sparkle.Application.GroupChats.Queries.GetGroupChatDetails;
using Sparkle.Application.Models;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.GroupChats.Queries
{
    public class GetGroupChatDetailsTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string chatId = Ids.GroupChat4;
            Guid userId = Ids.UserAId;

            SetAuthorizedUserId(userId);

            GetGroupChatDetailsQuery request = new() { ChatId = chatId };
            GetGroupChatDetailsQueryHandler handler = new(Context, UserProvider, Mapper);

            //Act
            PersonalChat chat = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.NotNull(chat);
            Assert.Contains(chat.Users, user => user == userId);
            Assert.Equal(chatId, chat.Id);
        }
    }
}
