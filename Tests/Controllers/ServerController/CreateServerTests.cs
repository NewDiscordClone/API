//using Application.Commands.Servers.CreateServer;
//using MediatR;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Tests.Common;
//using WebApi.Controllers;
//using WebApi.Models;

//namespace Tests.Controllers.ServerController
//{
//    public class CreateServerTests : TestBase
//    {
//        [Fact]
//        public async Task Success()
//        {
//            //Arrange
//            const int returnValue = 1;

//            Mock<IMediator> mediator = new();
//            mediator.Setup(mediator => mediator.Send(It.IsAny<CreateServerRequest>(), default))
//                .ReturnsAsync(returnValue);

//            ServersController controller = new(mediator.Object, UserProvider);

//            CrateServerDto dto = new()
//            { Title = "Test", Image = null };

//            //Act
//            ActionResult<int> result = await controller.CrateServer(dto);

//            //Assert
//            Assert.IsType<CreatedResult>(result.Result);
//            CreatedResult createdResult = (CreatedResult)result.Result;

//            Assert.Equal(returnValue, createdResult.Value);
//            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
//        }
//    }
//}
