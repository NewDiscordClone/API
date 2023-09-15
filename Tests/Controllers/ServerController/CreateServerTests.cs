using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Common.Servers.Commands.CreateServer;
using Sparkle.Application.Common.Servers.Queries.GetServerDetails;
using Sparkle.Tests.Common;
using Sparkle.WebApi.Controllers;
using System.Reflection;

namespace Sparkle.Tests.Controllers.ServerController
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
            Mock<IMediator> mockMediator = new();
            Mock<IMapper> mockMapper = new();
            mockMapper.Object.Config.Scan(Assembly.GetExecutingAssembly());

            ServersController controller = new(mockMediator.Object, UserProvider, mockMapper.Object);

            //Act
            ActionResult<string> result = await controller.CreateServer(request);

            //Assert
            CreatedResult createdResult = Assert.IsType<CreatedResult>(result.Result);
            ServerDetailsDto resultServer = await new GetServerDetailsCommandHandler(Context, UserProvider, Mapper)
                .Handle(new GetServerDetailsCommand() { ServerId = (string)createdResult.Value! }, CancellationToken);

            Assert.Equal(serverName, resultServer.Title);
            Assert.Contains(resultServer.ServerProfiles, sp => sp.UserId == userId);
            Assert.Equal(oldCount + 1, await Context.Servers.CountAsync(s => true));
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
        }
    }
}
