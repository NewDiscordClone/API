using Application.Models;
using Application.Models.LookUps;
using Application.Queries.GetPrivateChats;
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
            string usernames = "User B, User C, User D";

            SetAuthorizedUserId(userId);

            GetPrivateChatsRequest request = new();
            GetPrivateChatsRequestHandler requestHandler = new(Context, UserProvider, Mapper);

            //Act
            List<PrivateChatLookUp> chats = await requestHandler.Handle(request, CancellationToken);

            //Assert
            Assert.NotEmpty(chats);
            Assert.Equal(4, chats.Count);
            Assert.All(chats, chat => Assert.Contains(Context.Chats.FindAsync(chat.Id).Result.Users, user => user == userId));
            Assert.Contains(chats, c => c.Title == usernames);

        }
    }
}