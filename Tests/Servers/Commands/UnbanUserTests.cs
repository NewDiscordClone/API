﻿using Application.Common.Exceptions;
using Application.Common.Servers.Commands.UnbanUser;
using Application.Models;
using Tests.Common;

namespace Tests.Servers.Commands
{
    public class UnbanUserTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string serverId = Ids.Server1;
            Guid userId = Ids.UserDId;
            int oldCount = (await Context.Servers.FindAsync(serverId)).ServerProfiles.Count;

            SetAuthorizedUserId(Ids.UserAId);

            UnbanUserRequest request = new()
            {
                ServerId = serverId,
                UserId = userId
            };
            UnbanUserRequestHandler handler = new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);

            //Assert
            Server server = await Context.Servers.FindAsync(serverId);
            Assert.Equal(oldCount, server.ServerProfiles.Count);
            Assert.DoesNotContain(server.ServerProfiles, sp => sp.UserId == userId);
            Assert.DoesNotContain(server.BannedUsers, user => user == userId);
        }
        [Fact]
        public async Task Fail_DontHavePermissions()
        {
            //Arrange
            CreateDatabase();
            string serverId = Ids.Server1;
            Guid userId = Ids.UserDId;
            int oldCount = (await Context.Servers.FindAsync(serverId)).ServerProfiles.Count;

            SetAuthorizedUserId(Ids.UserCId);

            UnbanUserRequest request = new()
            {
                ServerId = serverId,
                UserId = userId
            };
            UnbanUserRequestHandler handler = new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(
                async () => await handler.Handle(request, CancellationToken));
            Server server = await Context.Servers.FindAsync(serverId);
            Assert.Equal(oldCount, server.ServerProfiles.Count);
            Assert.DoesNotContain(server.ServerProfiles, sp => sp.UserId == userId);
            Assert.Contains(server.BannedUsers, user => user == userId);
        }
    }
}