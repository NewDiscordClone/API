using Application.Models;
using Application.Models.LookUps;
using Application.Queries.GetPersonalChats;
using Tests.Common;

namespace Tests.GroupChats.Queries
{
    public class GetPrivateChatsListTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserAId;

            SetAuthorizedUserId(userId);

            GetPrivateChatsRequest request = new();
            GetPrivateChatsRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            List<PrivateChatLookUp> chats = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.NotEmpty(chats);
            Assert.Equal(3, chats.Count);
            Assert.All(chats, chat => Assert.Contains(Context.Chats.FindAsync(chat.Id).Result.Users, user => user == userId));
        }
    }
}