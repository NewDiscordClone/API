using Application.Providers;
using Application.Queries.GetServerDetails;
using Tests.Common;

namespace Tests.Servers.Queries
{
    public class GetServerDetailsTests : TestQueryBase
    {
        [Fact]
        public async Task GetServerDetails_Expected()
        {
            //Arrange
            int serverId = 1;
            int userId = 1;
            CancellationToken cancellationToken = CancellationToken.None;
            Mock<IAuthorizedUserProvider> mock = new();
            mock.Setup(mock => mock.GetUserId()).Returns(userId);
            GetServerDetailsRequestHandler handler = new(Context, mock.Object, Mapper);

            GetServerDetailsRequest request = new() { ServerId = serverId };

            //Act
            ServerDetailsDto result = await handler.Handle(request, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(serverId, result.Id);
            Assert.NotEmpty(result.Channels);
        }
    }
}
