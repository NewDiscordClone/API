using Application.Commands.Servers.UpdateServer;
using Application.Exceptions;
using Application.Models;
using Application.Providers;
using MongoDB.Bson;
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
            ObjectId userId = Ids.UserBId;
            ObjectId serverId = Ids.ServerIdForUpdate;
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
            Server updatedServer = await Context.FindByIdAsync<Server>(serverId, CancellationToken);

            //Assert
            Assert.NotNull(updatedServer);
            Assert.Equal(newTitle, updatedServer.Title);
        }

        [Fact]
        public async Task Unauthorized_Fail()
        {
            CreateDatabase();
            //Arrange
            ObjectId userId = Ids.UserAId;
            ObjectId serverId = Ids.ServerIdForUpdate;
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
