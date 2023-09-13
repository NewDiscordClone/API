using Application.Commands.Servers.DeleteServer;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tests.Common;
using WebApi.Controllers;

namespace Tests.Controllers.ServerController
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

            DeleteServerRequest request = new()
            {
                ServerId = serverId,
            };
            DeleteServerRequestHandler handler = new(Context, UserProvider);

            AddMediatorHandler(request, handler);

            ServersController controller = new(Mediator, UserProvider);

            //Act
            ActionResult result = await controller.DeleteServer(request);

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

            DeleteServerRequest request = new()
            {
                ServerId = serverId,
            };
            DeleteServerRequestHandler handler = new(Context, UserProvider);


            AddMediatorHandler(request, handler);

            ServersController controller = new(Mediator, UserProvider);

            //Act
            ActionResult result = await controller.DeleteServer(request);
            //Assert
            ForbidResult forbidResult = Assert.IsType<ForbidResult>(result);
        }
    }
}