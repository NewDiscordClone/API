using Sparkle.Application.GroupChats.Commands.CreateGroupChat;
using Sparkle.Application.Models;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.GroupChats.Commands
{
    public class CreateGroupChatTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserAId;
            List<Guid> userIdlist = new() { userId, Ids.UserDId };
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
            Assert.True(result.Users.Select(user => user).SequenceEqual(userIdlist));
        }
    }
}
