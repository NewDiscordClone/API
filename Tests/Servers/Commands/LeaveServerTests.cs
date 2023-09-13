using Application.Common.Exceptions;
using Application.Common.Servers.Commands.LeaveServer;
using Application.Models;
using Tests.Common;

namespace Tests.Servers.Commands
{
    public class LeaveServerTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string serverId = Ids.Server3;
            Guid userId = Ids.UserAId;
            int oldCount = (await Context.Servers.FindAsync(serverId)).ServerProfiles.Count;

            SetAuthorizedUserId(userId);

            LeaveServerRequest request = new()
            {
                ServerId = serverId
            };
            LeaveServerRequestHandler handler = new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);

            //Assert
            Server server = await Context.Servers.FindAsync(serverId);
            Assert.Equal(oldCount - 1, server.ServerProfiles.Count);
            Assert.DoesNotContain(server.ServerProfiles, sp => sp.UserId == userId);
        }
        [Fact]
        public async Task Fail_YouAreNotAMember()
        {
            //Arrange
            CreateDatabase();
            string serverId = Ids.Server3;
            Guid userId = Ids.UserDId;
            int oldCount = (await Context.Servers.FindAsync(serverId)).ServerProfiles.Count;

            SetAuthorizedUserId(userId);

            LeaveServerRequest request = new()
            {
                ServerId = serverId
            };
            LeaveServerRequestHandler handler = new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(
                async () => await handler.Handle(request, CancellationToken));
            Server server = await Context.Servers.FindAsync(serverId);
            Assert.Equal(oldCount, server.ServerProfiles.Count);
            Assert.DoesNotContain(server.ServerProfiles, sp => sp.UserId == userId);
        }
    }
}