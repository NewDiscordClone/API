﻿using Application.Commands.Servers.DeleteServer;
using Application.Exceptions;
using Application.Providers;
using MongoDB.Driver;
using Tests.Common;

namespace Tests.Servers.Commands
{
    public class DeleteServerTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            CreateDatabase();
            //Arrange
            int userId = Ids.UserAId;
            int serverId = Ids.ServerIdForDelete;
            int oldCount = Context.Servers.Count();

            SetAuthorizedUserId(userId);

            DeleteServerRequest request = new()
            {
                ServerId = serverId,
            };
            DeleteServerRequestHandler handler = new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);

            //Assert
            Assert.Null(Context.Servers.Find(serverId));
            Assert.False(Context.ServerProfiles.Any(profile => profile.Server.Id == serverId));
            Assert.False(Context.Channels.Find(channel => channel.ServerId == serverId).Any());
            Assert.Equal(oldCount - 1, Context.Servers.Count());
            Assert.NotNull(Context.Users.Find(userId));
        }

        [Fact]
        public async Task Unauthorized_Fail()
        {
            CreateDatabase();
            //Arrange
            int userId = Ids.UserBId;
            int serverId = Ids.ServerIdForDelete;

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
