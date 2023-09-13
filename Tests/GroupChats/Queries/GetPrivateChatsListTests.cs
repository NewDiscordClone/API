using Tests.Common;

namespace Tests.PrivateChats.Queries
{
    public class GetPrivateChatsListTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            int userId = Ids.UserAId;

            SetAuthorizedUserId(userId);

            GetPrivateChatsRequest request = new();
            GetPrivateChatsRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            List<PrivateChatLookUp> chats = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.NotEmpty(chats);
            Assert.Equal(3, chats.Count);
            Assert.All(chats, chat => Assert.Contains(chat.Users, user => user.Id == userId));
        }
    }
}