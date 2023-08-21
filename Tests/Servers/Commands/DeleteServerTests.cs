using Application.Commands.Servers.DeleteServer;
using Application.Exceptions;
using Application.Providers;
using Tests.Common;

namespace Tests.Servers.Commands
{
    public class DeleteServerTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            int userId = TestDbContextFactory.UserAId;
            int serverId = TestDbContextFactory.ServerIdForDelete;
            int oldCount = Context.Servers.Count();

            Mock<IAuthorizedUserProvider> userProvider = new();
            userProvider.Setup(p => p.GetUserId()).Returns(userId);

            DeleteServerRequest request = new()
            {
                ServerId = serverId,
            };
            DeleteServerRequestHandler handler = new(Context, userProvider.Object);

            //Act
            await handler.Handle(request, CancellationToken);

            //Assert
            Assert.Null(Context.Servers.Find(serverId));
            Assert.False(Context.ServerProfiles.Any(profile => profile.Server.Id == serverId));
            Assert.False(Context.Channels.Any(channel => channel.Server.Id == serverId));
            Assert.Equal(oldCount - 1, Context.Servers.Count());
            Assert.NotNull(Context.Users.Find(userId));
        }

        [Fact]
        public async Task Unauthorized_Fail()
        {
            //Arrange
            int userId = TestDbContextFactory.UserBId;
            int serverId = TestDbContextFactory.ServerIdForDelete;

            Mock<IAuthorizedUserProvider> userProvider = new();
            userProvider.Setup(p => p.GetUserId()).Returns(userId);

            DeleteServerRequest request = new()
            {
                ServerId = serverId,
            };
            DeleteServerRequestHandler handler = new(Context, userProvider.Object);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(async () => await handler.Handle(request, CancellationToken));
        }
    }
}
