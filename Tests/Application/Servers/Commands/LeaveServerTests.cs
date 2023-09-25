using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;
using Sparkle.Application.Servers.Commands.LeaveServer;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Application.Servers.Commands
{
    public class LeaveServerTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string serverId = Ids.Server3;
            Guid userId = Ids.UserAId;
            int oldCount = (await Context.Servers.FindAsync(serverId)).Profiles.Count;

            SetAuthorizedUserId(userId);

            Guid profileId = default;
            LeaveServerCommand request = new()
            {
                ServerId = serverId,
                ProfileId = profileId
            };

            Mock<IServerProfileRepository> repository = new();

            LeaveServerCommandHandler handler = new(Context, UserProvider, repository.Object);

            //Act
            await handler.Handle(request, CancellationToken);

            //Assert
            Server server = await Context.Servers.FindAsync(serverId);
            Assert.Equal(oldCount - 1, server.Profiles.Count);
            Assert.DoesNotContain(Context.UserProfiles, sp => sp.Id == profileId);
        }
    }
}