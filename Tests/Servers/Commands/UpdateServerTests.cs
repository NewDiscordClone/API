using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Models;
using Sparkle.Application.Servers.Commands.UpdateServer;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Servers.Commands
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

            UpdateServerCommand request = new()
            {
                ServerId = serverId,
                Image = null,
                Title = newTitle
            };
            UpdateServerCommandHandler handler = new(Context, UserProvider);

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

            UpdateServerCommand request = new()
            {
                ServerId = serverId,
                Image = null,
                Title = newTitle
            };
            UpdateServerCommandHandler handler = new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(async () => await handler.Handle(request, CancellationToken));
        }
    }
}
