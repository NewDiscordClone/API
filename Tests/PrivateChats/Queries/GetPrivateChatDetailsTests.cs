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
            const int chatId = 4;
            int userId = TestDbContextFactory.UserAId;

            Mock<IAuthorizedUserProvider> userProvider = new();
            userProvider.Setup(provider => provider.GetUserId()).Returns(userId);

            GetPrivateChatDetailsRequest request = new() { ChatId = chatId };
            GetPrivateChatDetailsRequestHandler handler = new(Context, userProvider.Object, Mapper);

            //Act
            GetPrivateChatDetailsDto chat = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.NotNull(chat);
            Assert.Contains(chat.Users, user => user.Id == userId);
            Assert.Equal(chatId, chat.Id);
        }
    }
}
