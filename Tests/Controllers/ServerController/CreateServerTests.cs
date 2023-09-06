using System.Net.Http.Headers;
using Application.Commands.Servers.CreateServer;
using Application.Queries.GetServerDetails;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tests.Common;
using WebApi.Controllers;

namespace Tests.Controllers.ServerController
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

            AddMediatorHandler(request, handler);

            ServersController controller = new(Mediator, UserProvider);

            //Act
            ActionResult<string> result = await controller.CreateServer(request);

            //Assert
            CreatedResult createdResult = Assert.IsType<CreatedResult>(result.Result);
            ServerDetailsDto resultServer = await new GetServerDetailsRequestHandler(Context, UserProvider, Mapper)
                .Handle(new GetServerDetailsRequest() { ServerId = (string)createdResult.Value }, CancellationToken);

            Assert.Equal(serverName, resultServer.Title);
            Assert.Contains(resultServer.ServerProfiles, sp => sp.UserId == userId);
            Assert.Equal(oldCount + 1, await Context.Servers.CountAsync(s => true));
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
        }
    }
}
