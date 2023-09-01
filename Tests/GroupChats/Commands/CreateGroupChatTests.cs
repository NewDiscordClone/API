using Application.Commands.GroupChats.CreateGroupChat;
using Application.Models;
using Tests.Common;

namespace Tests.GroupChats.Commands
{
    public class CreateGroupChatTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            int userId = Ids.UserAId;
            List<int> userIdlist = new() { userId, Ids.UserDId };
            const string title = "TestCreate";

            SetAuthorizedUserId(userId);

            CreateGroupChatRequest request = new()
            {
                UsersId = userIdlist,
                Title = title,
            };
            CreateGroupChatRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            string id = await handler.Handle(request, CancellationToken);
            GroupChat result = await Context.GroupChats.FindAsync(id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(title, result.Title);
            Assert.True(result.Users.Select(user => user.Id).SequenceEqual(userIdlist));
        }
    }
}
