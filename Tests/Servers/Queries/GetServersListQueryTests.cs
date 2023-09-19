using Sparkle.Application.Servers.Queries.ServersList;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Servers.Queries
{
    public class GetServersListQueryTests : TestBase
    {
        [Fact]
        public async Task Handle_ReturnsExpectedServers()
        {
            // Arrange
            CreateDatabase();
            Guid userId = Ids.UserAId;
            CancellationToken cancellationToken = CancellationToken.None;

            SetAuthorizedUserId(userId);
            ServersListQueryHandler handler = new(Context, UserProvider);

            ServersListQuery request = new();

            // Act
            List<ServerLookUpDto> result = await handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Server 1", result[0].Title);
        }
        [Fact]
        public async Task Handle_ReturnsUnexpectedServers()
        {
            // Arrange
            CreateDatabase();
            Guid userId = Guid.NewGuid();
            CancellationToken cancellationToken = CancellationToken.None;

            SetAuthorizedUserId(userId);

            ServersListQueryHandler handler = new(Context, UserProvider);

            ServersListQuery request = new();

            // Act
            List<ServerLookUpDto> result = await handler.Handle(request, cancellationToken);

            // Assert
            Assert.Empty(result);
        }
    }
}
