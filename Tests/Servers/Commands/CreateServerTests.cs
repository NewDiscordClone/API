using Application.Common.Servers.Commands.CreateServer;
using Application.Common.Servers.Queries.GetServerDetails;
using Tests.Common;

namespace Tests.Servers.Commands
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

            CreateServerRequest request = new() { Title = serverName, Image = null };
            CreateServerRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act

            string result = await handler.Handle(request, CancellationToken);
            //Assert
            ServerDetailsDto resultServer = await new GetServerDetailsRequestHandler(Context, UserProvider, Mapper)
                .Handle(new GetServerDetailsRequest() { ServerId = result }, CancellationToken);

            Assert.Equal(serverName, resultServer.Title);
            Assert.Contains(resultServer.ServerProfiles, sp => sp.UserId == userId);
            Assert.Equal(oldCount + 1, await Context.Servers.CountAsync(s => true));
        }
    }
}
