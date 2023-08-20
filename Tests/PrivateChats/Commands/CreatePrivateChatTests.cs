using Application.Commands.PrivateChats.CreatePrivateChat;
using Application.Models;
using Tests.Common;

namespace Tests.PrivateChats.Commands
{
    public class CreatePrivateChatTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            int userId = TestDbContextFactory.UserAId;
            List<int> userIdlist = new() { userId, TestDbContextFactory.UserDId };
            const string title = "TestCreate";

            SetAuthorizedUserId(userId);

            CreatePrivateChatRequest request = new()
            {
                UsersId = userIdlist,
                Title = title,
            };
            CreatePrivateChatRequestHandler handler = new(Context, UserProvider);

            //Act
            PrivateChat result = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(title, result.Title);
            Assert.True(result.Users.Select(user => user.Id).SequenceEqual(userIdlist));
        }
    }
}
