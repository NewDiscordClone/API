using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Models;
using Sparkle.Contracts.Servers;
using Sparkle.Tests.Common;
using Sparkle.WebApi.Controllers;
using System.Reflection;

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
                Image = null,
                Title = newTitle
            };

            Mock<IMediator> mockMediator = new();
            Mock<IMapper> mockMapper = new();
            mockMapper.Object.Config.Scan(Assembly.GetExecutingAssembly());

            ServersController controller = new(mockMediator.Object, UserProvider, mockMapper.Object);

            //Act
            ActionResult result = await controller.UpdateServer(serverId, request);

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
                Image = null,
                Title = newTitle
            };
            Mock<IMediator> mockMediator = new();

            Mock<IMapper> mockMapper = new();
            mockMapper.Object.Config.Scan(Assembly.GetExecutingAssembly());

            ServersController controller = new(mockMediator.Object, UserProvider, mockMapper.Object);

            ActionResult result = await controller.UpdateServer(serverId, request);
            //Act
            //Assert
            Assert.IsType<ForbidResult>(result);
        }
    }
}
