using Sparkle.Application.Servers.Queries.ServerDetails;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Application.Servers.Queries
{
    public class GetServerDetailsTests : TestBase
    {
        [Fact]
        public async Task GetServerDetails_Expected()
        {
            //Arrange
            CreateDatabase();
            string serverId = Ids.Server1;
            Guid userId = Ids.UserAId;
            CancellationToken cancellationToken = CancellationToken.None;

            SetAuthorizedUserId(userId);

            ServerDetailsQueryHandler handler = new(Context, UserProvider, Mapper);

            ServerDetailsQuery request = new() { ServerId = serverId };

            //Act
            ServerDetailsDto result = await handler.Handle(request, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(serverId, result.Id);
            Assert.NotEmpty(result.Channels);
            Assert.Contains(result.ServerProfiles, sp => sp.UserId == userId);
        }
    }
}
