using Sparkle.Application.Servers.Commands.CreateServer;
using Sparkle.Application.Servers.Queries.GetServerDetails;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Servers.Commands
{
    public class CreateServerTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserAId;
            long oldCount = await Context.Servers.CountAsync(s => true);
            const string serverName = "New server";

            SetAuthorizedUserId(userId);

            CreateServerCommand request = new() { Title = serverName, Image = null };
            CreateServerCommandHandler handler = new(Context, UserProvider, Mapper);

            //Act

            string result = await handler.Handle(request, CancellationToken);
            //Assert
            ServerDetailsDto resultServer = await new GetServerDetailsCommandHandler(Context, UserProvider, Mapper)
                .Handle(new GetServerDetailsCommand() { ServerId = result }, CancellationToken);

            Assert.Equal(serverName, resultServer.Title);
            Assert.Contains(resultServer.ServerProfiles, sp => sp.UserId == userId);
            Assert.Equal(oldCount + 1, await Context.Servers.CountAsync(s => true));
        }
    }
}
