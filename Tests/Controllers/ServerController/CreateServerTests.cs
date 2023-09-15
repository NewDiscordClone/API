using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Servers.Commands.CreateServer;
using Sparkle.Tests.Common;
using Sparkle.WebApi.Controllers;

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
            const string serverName = "New server";
            const string serverId = "5f95a3c3d0ddad0017ea9291";

            SetAuthorizedUserId(userId);

            CreateServerCommand request = new() { Title = serverName, Image = null };

            MediatorMock.Setup(mediator => mediator.Send(It.IsAny<CreateServerCommand>(), CancellationToken))
                .ReturnsAsync(serverId);
            Mock<IMapper> mapper = new();

            ServersController controller = new(Mediator, UserProvider, mapper.Object);

            //Act
            ActionResult<string> result = await controller.CreateServer(request);

            //Assert
            CreatedAtActionResult createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(serverId, createdResult.Value);
            MediatorMock.Verify(mediator => mediator
            .Send(It.IsAny<CreateServerCommand>(), CancellationToken)
            , Times.Once);
        }
    }
}
