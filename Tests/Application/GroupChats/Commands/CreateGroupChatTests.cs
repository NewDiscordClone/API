using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.GroupChats.Commands.CreateGroupChat;
using Sparkle.Application.Models;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Application.GroupChats.Commands
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

            CreateGroupChatCommand request = new()
            {
                UsersId = userIdlist,
                Title = title,
            };
            Mock<IUserProfileRepository> mock = new();
            CreateGroupChatCommandHandler handler = new(Context, UserProvider, Mapper, mock.Object);

            //Act

            string id = await handler.Handle(request, CancellationToken);
            GroupChat result = await Context.GroupChats.FindAsync(id);
            userIdlist.Add(userId);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(title, result.Title);
            Assert.Equal(userIdlist.Count + 1, result.Profiles.Count);
        }
    }
}
