using Application.Commands.Servers.KickUser;
using Application.Common.Exceptions;
using Application.Models;
using Tests.Common;

namespace Tests.Servers.Commands
{
    public class KickUserTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string serverId = Ids.Server3;
            Guid userId = Ids.UserAId;
            int oldCount = (await Context.Servers.FindAsync(serverId)).ServerProfiles.Count;

            SetAuthorizedUserId(Ids.UserCId);

            KickUserRequest request = new()
            {
                ServerId = serverId,
                UserId = userId
            };
            KickUserRequestHandler handler = new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);

            //Assert
            Server server = await Context.Servers.FindAsync(serverId);
            Assert.Equal(oldCount - 1, server.ServerProfiles.Count);
            Assert.DoesNotContain(server.ServerProfiles, sp => sp.UserId == userId);
        }
        [Fact]
        public async Task Fail_DontHavePermissions()
        {
            //Arrange
            CreateDatabase();
            string serverId = Ids.Server3;
            Guid userId = Ids.UserAId;
            int oldCount = (await Context.Servers.FindAsync(serverId)).ServerProfiles.Count;

            SetAuthorizedUserId(Ids.UserBId);

            KickUserRequest request = new()
            {
                ServerId = serverId,
                UserId = userId
            };
            KickUserRequestHandler handler = new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(
                async () => await handler.Handle(request, CancellationToken));
            Server server = await Context.Servers.FindAsync(serverId);
            Assert.Equal(oldCount, server.ServerProfiles.Count);
            Assert.Contains(server.ServerProfiles, sp => sp.UserId == userId);
        }
        [Fact]
        public async Task Fail_UserAreNotAMember()
        {
            //Arrange
            CreateDatabase();
            string serverId = Ids.Server3;
            Guid userId = Ids.UserDId;
            int oldCount = (await Context.Servers.FindAsync(serverId)).ServerProfiles.Count;

            SetAuthorizedUserId(Ids.UserCId);

            KickUserRequest request = new()
            {
                ServerId = serverId,
                UserId = userId
            };
            KickUserRequestHandler handler = new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<Exception>(
                async () => await handler.Handle(request, CancellationToken));
            Server server = await Context.Servers.FindAsync(serverId);
            Assert.Equal(oldCount, server.ServerProfiles.Count);
            Assert.DoesNotContain(server.ServerProfiles, sp => sp.UserId == userId);
        }
    }
}