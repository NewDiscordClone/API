using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Servers.Commands.DeleteServer;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Servers.Commands
{
    public class DeleteServerTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserAId;
            string serverId = Ids.Server1;
            long oldCount = await Context.Servers.CountAsync(s => true);

            SetAuthorizedUserId(userId);

            DeleteServerCommand request = new()
            {
                ServerId = serverId,
            };
            DeleteServerRequestHandler handler = new(Context, UserProvider);

            //Act

            await handler.Handle(request, CancellationToken);

            //Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await Context.Servers.FindAsync(serverId));
            Assert.Empty(await Context.Channels.FilterAsync(channel => channel.ServerId == serverId));
            Assert.Equal(oldCount - 1, await Context.Servers.CountAsync(s => true));
            Assert.NotNull(await Context.SqlUsers.FindAsync(userId));
        }

        [Fact]
        public async Task Unauthorized_Fail()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserBId;
            string serverId = Ids.Server1;

            SetAuthorizedUserId(userId);

            DeleteServerCommand request = new()
            {
                ServerId = serverId,
            };
            DeleteServerRequestHandler handler = new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(async () =>
                await handler.Handle(request, CancellationToken));
        }
    }
}