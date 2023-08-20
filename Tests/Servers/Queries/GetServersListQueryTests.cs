using Application.Queries.GetServer;
using Tests.Common;

namespace Tests.Servers.Queries
{
    public class GetServersListQueryTests : TestBase
    {
        [Fact]
        public async Task Handle_ReturnsExpectedServers()
        {
            // Arrange
            int userId = 1;
            CancellationToken cancellationToken = CancellationToken.None;

            GetServersRequestHandler handler = new(Context, Mapper);

            GetServersRequest request = new()
            { UserId = userId };

            // Act
            List<GetServerLookupDto> result = await handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Server 1", result[0].Title);
        }
        [Fact]
        public async Task Handle_ReturnsUnexpectedServers()
        {
            // Arrange
            int userId = int.MaxValue;
            CancellationToken cancellationToken = CancellationToken.None;

            GetServersRequestHandler handler = new(Context, Mapper);

            GetServersRequest request = new()
            { UserId = userId };

            // Act
            List<GetServerLookupDto> result = await handler.Handle(request, cancellationToken);

            // Assert
            Assert.Empty(result);
        }


    }
}
