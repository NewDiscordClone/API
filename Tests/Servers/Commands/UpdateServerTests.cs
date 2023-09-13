using Application.Commands.Servers.UpdateServer;
using Application.Common.Exceptions;
using Application.Models;
using Tests.Common;

namespace Tests.Servers.Commands
{
    public class UpdateServerTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserBId;
            string serverId = Ids.Server2;
            const string newTitle = "Updated title";

            SetAuthorizedUserId(userId);

            UpdateServerRequest request = new()
            {
                ServerId = serverId,
                Image = null,
                Title = newTitle
            };
            UpdateServerRequestHandler handler = new(Context, UserProvider);

            //Act
            Context.SetToken(CancellationToken);
            await handler.Handle(request, CancellationToken);
            //Assert
            Server updatedServer = await Context.Servers.FindAsync(serverId);
            Assert.NotNull(updatedServer);
            Assert.Equal(newTitle, updatedServer.Title);
        }

        [Fact]
        public async Task Unauthorized_Fail()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserAId;
            string serverId = Ids.Server2;
            const string newTitle = "Updated title";

            SetAuthorizedUserId(userId);

            UpdateServerRequest request = new()
            {
                ServerId = serverId,
                Image = null,
                Title = newTitle
            };
            UpdateServerRequestHandler handler = new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(async () => await handler.Handle(request, CancellationToken));
        }
    }
}
