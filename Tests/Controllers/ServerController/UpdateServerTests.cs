using Application.Commands.Servers.UpdateServer;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tests.Common;
using WebApi.Controllers;

namespace Tests.Controllers.ServerController
{
    public class UpdateServerTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            string serverId = Ids.ServerIdForUpdate;

            Mock<IMediator> mediator = new();
            mediator.Setup(mediator => mediator.Send(It.IsAny<UpdateServerRequest>(), CancellationToken))
                .Returns(Task.CompletedTask);

            ServersController controller = new(mediator.Object, UserProvider);
            UpdateServerRequest request = new()
            {
                Image = null,
                Title = "new title",
                ServerId = serverId
            };

            //Act
            ActionResult result = await controller.UpdateServer(request);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
