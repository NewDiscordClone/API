using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Common.Servers.Commands.UpdateServer;
using Sparkle.Application.Models;
using Sparkle.Tests.Common;
using Sparkle.WebApi.Controllers;

namespace Sparkle.Tests.Controllers.ServerController
{
    public class UpdateServerTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserBId;
            string serverId = Ids.Server2;
            const string newTitle = "Updated title";

            SetAuthorizedUserId(userId);

            UpdateServerRequest request = new()
            {
                ServerId = serverId,
                Image = null,
                Title = newTitle
            };
            UpdateServerCommandHandler handler = new(Context, UserProvider);

            AddMediatorHandler(request, handler);

            ServersController controller = new(Mediator, UserProvider);

            //Act
            ActionResult result = await controller.UpdateServer(request);

            //Assert
            NoContentResult noContent = Assert.IsType<NoContentResult>(result);
            Server updatedServer = await Context.Servers.FindAsync(serverId);
            Assert.NotNull(updatedServer);
            Assert.Equal(newTitle, updatedServer.Title);
            Assert.Equal(StatusCodes.Status204NoContent, noContent.StatusCode);
        }
        [Fact]
        public async Task Unauthorized_Fail()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserAId;
            string serverId = Ids.Server2;
            const string newTitle = "Updated title";

            SetAuthorizedUserId(userId);

            UpdateServerRequest request = new()
            {
                ServerId = serverId,
                Image = null,
                Title = newTitle
            };
            UpdateServerCommandHandler handler = new(Context, UserProvider);

            AddMediatorHandler(request, handler);

            ServersController controller = new(Mediator, UserProvider);

            ActionResult result = await controller.UpdateServer(request);
            //Act
            //Assert
            ForbidResult forbidResult = Assert.IsType<ForbidResult>(result);
        }
    }
}
