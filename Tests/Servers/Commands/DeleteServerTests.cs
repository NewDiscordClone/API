using Application.Commands.Servers.DeleteServer;
using Application.Common.Exceptions;
using Application.Models;
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
            CreateDatabase();
            int userId = Ids.UserAId;
            string serverId = Ids.ServerIdForDelete;
            long oldCount = await Context.Servers.CountAsync(s => true);


            SetAuthorizedUserId(userId);

            DeleteServerRequest request = new()
            {
                ServerId = serverId,
            };
            DeleteServerRequestHandler handler = new(Context, UserProvider);

            //Act
            Context.SetToken(CancellationToken);
            await handler.Handle(request, CancellationToken);

            //Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await Context.Servers.FindAsync(serverId));
            Assert.Empty(await Context.Channels.FilterAsync(channel => channel.ServerId == serverId));
            Assert.Equal(oldCount - 1, await Context.Servers.CountAsync(s => true));
            Assert.NotNull(await Context.FindSqlByIdAsync<User>(userId, CancellationToken));
        }

        [Fact]
        public async Task Unauthorized_Fail()
        {
            //Arrange
            CreateDatabase();
            int userId = Ids.UserBId;
            string serverId = Ids.ServerIdForDelete;

            Mock<IAuthorizedUserProvider> userProvider = new();
            userProvider.Setup(p => p.GetUserId()).Returns(userId);

            DeleteServerRequest request = new()
            {
                ServerId = serverId,
            };
            DeleteServerRequestHandler handler = new(Context, userProvider.Object);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(async () =>
                await handler.Handle(request, CancellationToken));
        }
    }
}