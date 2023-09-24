﻿using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Models;
using Sparkle.Application.Servers.Commands.BanUser;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Application.Servers.Commands
{
    public class BanUserTests : TestBase
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

            BanUserCommand request = new()
            {
                ServerId = serverId,
                UserId = userId
            };
            BanUserCommandHandler handler = new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);

            //Assert
            Server server = await Context.Servers.FindAsync(serverId);
            Assert.Equal(oldCount - 1, server.ServerProfiles.Count);
            Assert.DoesNotContain(server.ServerProfiles, sp => sp.UserId == userId);
            Assert.Contains(server.BannedUsers, user => user == userId);
        }
        [Fact]
        public async Task Fail_YouDontHaveRights()
        {
            //Arrange
            CreateDatabase();
            string serverId = Ids.Server3;
            Guid userId = Ids.UserAId;
            int oldCount = (await Context.Servers.FindAsync(serverId)).ServerProfiles.Count;

            SetAuthorizedUserId(Ids.UserBId);

            BanUserCommand request = new()
            {
                ServerId = serverId,
                UserId = userId
            };
            BanUserCommandHandler handler = new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(
                async () => await handler.Handle(request, CancellationToken));
            Server server = await Context.Servers.FindAsync(serverId);
            Assert.Equal(oldCount, server.ServerProfiles.Count);
            Assert.Contains(server.ServerProfiles, sp => sp.UserId == userId);
            Assert.DoesNotContain(server.BannedUsers, user => user == userId);
        }
        [Fact]
        public async Task Success_UserAreNotAMember()
        {
            //Arrange
            CreateDatabase();
            string serverId = Ids.Server3;
            Guid userId = Ids.UserDId;
            int oldCount = (await Context.Servers.FindAsync(serverId)).ServerProfiles.Count;

            SetAuthorizedUserId(Ids.UserCId);

            BanUserCommand request = new()
            {
                ServerId = serverId,
                UserId = userId
            };
            BanUserCommandHandler handler = new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);
            //Assert
            Server server = await Context.Servers.FindAsync(serverId);
            Assert.Equal(oldCount, server.ServerProfiles.Count);
            Assert.DoesNotContain(server.ServerProfiles, sp => sp.UserId == userId);
            Assert.Contains(server.BannedUsers, user => user == userId);
        }
    }
}