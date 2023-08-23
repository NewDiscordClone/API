using Application.Commands.Servers.CreateServer;
using Application.Providers;
using Application.Queries.GetServerDetails;
using Tests.Common;

namespace Tests.Servers.Commands
{
    public class CreateServerTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            int userId = TestDbContextFactory.UserAId;
            int oldCount = Context.Servers.Count();
            const string serverName = "New server";

            SetAuthorizedUserId(userId);

            CreateServerRequest request = new() { Title = serverName, Image = null };
            CreateServerRequestHandler handler = new(Context, UserProvider);

            //Act
            int result = await handler.Handle(request, CancellationToken);
            ServerDetailsDto resultServer = await new GetServerDetailsRequestHandler(Context, UserProvider, Mapper)
                .Handle(new GetServerDetailsRequest() { ServerId = result }, CancellationToken);

            //Assert
            Assert.Equal(serverName, resultServer.Title);
            Assert.NotNull(resultServer.ServerProfiles.FirstOrDefault(profile => profile.UserId == userId));
            Assert.Equal(oldCount + 1, Context.Servers.Count());
        }
    }
}
