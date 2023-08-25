using Application.Commands.Servers.DeleteServer;
using Application.Exceptions;
using Application.Models;
using Application.Providers;
using MongoDB.Bson;
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
            ObjectId serverId = Ids.ServerIdForDelete;
            long oldCount = await Context.Servers.CountDocumentsAsync(Builders<Server>.Filter.Empty);
            

            SetAuthorizedUserId(userId);

            DeleteServerRequest request = new()
            {
                ServerId = serverId,
            };
            DeleteServerRequestHandler handler = new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);

            //Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await Context.FindByIdAsync<Server>(serverId, CancellationToken));
            Assert.False(Context.Channels.Find(channel => channel.ServerId == serverId).Any());
            Assert.Equal(oldCount - 1, await Context.Servers.CountDocumentsAsync(Builders<Server>.Filter.Empty));
            Assert.NotNull(await Context.FindSqlByIdAsync<User>(userId, CancellationToken));
        }

        [Fact]
        public async Task Unauthorized_Fail()
        {
            CreateDatabase();
            //Arrange
            int userId = Ids.UserBId;
            ObjectId serverId = Ids.ServerIdForDelete;

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