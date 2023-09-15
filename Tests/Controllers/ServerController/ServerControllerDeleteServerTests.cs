using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Servers.Commands.DeleteServer;
using Sparkle.Tests.Common;
using Sparkle.WebApi.Controllers;

namespace Sparkle.Tests.Controllers.ServerController
{
    public class ServerControllerDeleteServerTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserAId;
            string serverId = Ids.Server1;
            long oldCount = await Context.Servers.CountAsync(s => true);


            SetAuthorizedUserId(userId);

            Mock<IMapper> mockMapper = new();

            ServersController controller = new(Mediator, UserProvider, mockMapper.Object);

            //Act
            ActionResult result = await controller.DeleteServer(serverId);

            //Assert
            NoContentResult noContentResult = Assert.IsType<NoContentResult>(result);
            // check command runes one time mediator mock
            MediatorMock.Verify(mediator => mediator
             .Send(It.IsAny<DeleteServerCommand>(), CancellationToken)
             , Times.Once);

        }
    }
}