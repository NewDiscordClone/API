using Application.Queries.GetPrivateChats;
using Tests.Common;

namespace Tests.PrivateChats.Queries
{
    public class GetPrivateChatsListTests : TestQueryBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            int userId = TestDbContextFactory.UserAId;

            GetPrivateChatsRequest request = new()
            {
                UserId = userId
            };
            GetPrivateChatsRequestHandler handler = new(Context, Mapper);

            //Act
            List<GetPrivateChatLookUpDto> chats = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.NotEmpty(chats);
            Assert.Equal(3, chats.Count);
            Assert.All(chats, chat => Assert.Contains(chat.Users, user => user.Id == userId));
        }
    }
}
