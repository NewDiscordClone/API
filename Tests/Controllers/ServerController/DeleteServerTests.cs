using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Tests.Common;
using Sparkle.WebApi.Controllers;
using System.Reflection;

namespace Sparkle.Tests.Controllers.ServerController
{
    public class DeleteServerTests : TestBase
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

            Mock<IMediator> mockMediator = new();
            Mock<IMapper> mockMapper = new();
            mockMapper.Object.Config.Scan(Assembly.GetExecutingAssembly());

            ServersController controller = new(mockMediator.Object, UserProvider, mockMapper.Object);

            //Act
            ActionResult result = await controller.DeleteServer(serverId);

            //Assert
            NoContentResult noContentResult = Assert.IsType<NoContentResult>(result);
            await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await Context.Servers.FindAsync(serverId));
            Assert.Empty(await Context.Channels.FilterAsync(channel => channel.ServerId == serverId));
            Assert.Equal(oldCount - 1, await Context.Servers.CountAsync(s => true));
            Assert.NotNull(await Context.SqlUsers.FindAsync(userId));
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }
        [Fact]
        public async Task Unauthorized_Fail()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserBId;
            string serverId = Ids.Server1;

            SetAuthorizedUserId(userId);

            Mock<IMediator> mockMediator = new();
            Mock<IMapper> mockMapper = new();
            mockMapper.Object.Config.Scan(Assembly.GetExecutingAssembly());

            ServersController controller = new(mockMediator.Object, UserProvider, mockMapper.Object);

            //Act
            ActionResult result = await controller.DeleteServer(serverId);
            //Assert
            ForbidResult forbidResult = Assert.IsType<ForbidResult>(result);
        }
    }
}