using Sparkle.Application.GroupChats.Queries.PrivateChatsList;
using Sparkle.Application.Models.LookUps;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.GroupChats.Queries
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

            PrivateChatsQuery request = new();
            PrivateChatsQueryHandler handler = new(Context, UserProvider, Mapper);

            //Act
            List<PrivateChatLookUp> chats = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.NotEmpty(chats);
            Assert.Equal(4, chats.Count);
        }
    }
}