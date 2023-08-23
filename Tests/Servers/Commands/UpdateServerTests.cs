﻿using Application.Commands.Servers.UpdateServer;
using Application.Exceptions;
using Application.Models;
using Application.Providers;
using Tests.Common;

namespace Tests.Servers.Commands
{
    public class UpdateServerTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            CreateDatabase();
            //Arrange
            int userId = Ids.UserBId;
            int serverId = Ids.ServerIdForUpdate;
            const string newTitle = "Updated title";

            Mock<IAuthorizedUserProvider> userProvider = new();
            userProvider.Setup(p => p.GetUserId()).Returns(userId);

            UpdateServerRequest request = new()
            {
                ServerId = serverId,
                Image = null,
                Title = newTitle
            };
            UpdateServerRequestHandler handler = new(Context, userProvider.Object);

            //Act
            await handler.Handle(request, CancellationToken);
            Server? updatedServer = Context.Servers.Find(serverId);

            //Assert
            Assert.NotNull(updatedServer);
            Assert.Equal(newTitle, updatedServer.Title);
        }

        [Fact]
        public async Task Unauthorized_Fail()
        {
            CreateDatabase();
            //Arrange
            int userId = Ids.UserAId;
            int serverId = Ids.ServerIdForUpdate;
            const string newTitle = "Updated title";

            Mock<IAuthorizedUserProvider> userProvider = new();
            userProvider.Setup(p => p.GetUserId()).Returns(userId);

            UpdateServerRequest request = new()
            {
                ServerId = serverId,
                Image = null,
                Title = newTitle
            };
            UpdateServerRequestHandler handler = new(Context, userProvider.Object);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(async () => await handler.Handle(request, CancellationToken));
        }
    }
}
