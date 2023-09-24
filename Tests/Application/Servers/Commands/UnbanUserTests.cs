﻿using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Models;
using Sparkle.Application.Servers.Commands.UnbanUser;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Application.Servers.Commands
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

            UnbanUserCommand request = new()
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

            UnbanUserCommand request = new()
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