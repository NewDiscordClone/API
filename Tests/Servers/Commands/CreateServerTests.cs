using Application.Commands.Servers.CreateServer;
using Application.Models;
using Application.Interfaces;
using Application.Queries.GetServerDetails;
using MongoDB.Bson;
using MongoDB.Driver;
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
            int userId = Ids.UserAId;
            long oldCount = await Context.Servers.CountAsync(s => true);
            const string serverName = "New server";

            SetAuthorizedUserId(userId);

            CreateServerRequest request = new() { Title = serverName, Image = null };
            CreateServerRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            Context.SetToken(CancellationToken);
            string result = await handler.Handle(request, CancellationToken);
            ServerDetailsDto resultServer = await new GetServerDetailsRequestHandler(Context, UserProvider, Mapper)
                .Handle(new GetServerDetailsRequest() { ServerId = result }, CancellationToken);

            //Assert
            Assert.Equal(serverName, resultServer.Title);
            Assert.NotNull(resultServer.ServerProfiles.FirstOrDefault(profile => profile.UserId == userId));
            Assert.Equal(oldCount + 1, await Context.Servers.CountAsync(s => true));
        }
    }
}
