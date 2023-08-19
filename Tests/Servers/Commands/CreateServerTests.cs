using Application.Commands.Servers.CreateServer;
using Application.Providers;
using Application.Queries.GetServerDetails;
using Tests.Common;

namespace Tests.Servers.Commands
{
    public class CreateServerTests : TestQueryBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            const int userId = 1;
            int oldCount = Context.Servers.Count();
            const string serverName = "New server";

            Mock<IAuthorizedUserProvider> userProvider = new();
            userProvider.Setup(provider => provider.GetUserId()).Returns(userId);

            CreateServerRequest request = new() { Title = serverName, Image = null };
            CreateServerRequestHandler handler = new(Context, userProvider.Object);

            //Act
            int result = await handler.Handle(request, CancellationToken);
            ServerDetailsDto resultServer = await new GetServerDetailsRequestHandler(Context, userProvider.Object, Mapper)
                .Handle(new GetServerDetailsRequest() { ServerId = result }, CancellationToken);

            //Assert
            Assert.Equal(serverName, resultServer.Title);
            Assert.NotNull(resultServer.ServerProfiles.FirstOrDefault(profile => profile.UserId == userId));
            Assert.Equal(oldCount + 1, Context.Servers.Count());
        }
    }
}
