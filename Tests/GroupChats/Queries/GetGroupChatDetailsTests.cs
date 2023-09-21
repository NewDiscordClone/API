using Sparkle.Application.GroupChats.Queries.GroupChatDetails;
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

            GroupChatDetailsQuery request = new() { ChatId = chatId };
            GroupChatDetailsQueryHandler handler = new(Context, UserProvider, Mapper);

            //Act
            PersonalChat chat = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.NotNull(chat);
            Assert.Contains(chat.Profiles, profile => profile.UserId == userId);
            Assert.Equal(chatId, chat.Id);
        }
    }
}
