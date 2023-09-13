using Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using ExceptionFilterAttribute = WebApi.Attributes.ExceptionFilterAttribute;

namespace Tests
{
    public class ExceptionFilterTests
    {
        private readonly ActionContext _actionContext = new()
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };
        public async Task<IActionResult?> GetResultAsync(ExceptionFilterAttribute attribute, Exception exception)
        {
            ExceptionContext context = new(_actionContext, new List<IFilterMetadata>())
            {
                Exception = exception,
            };
            await attribute.OnExceptionAsync(context);
            return context.Result;
        }
        [Fact]
        public async Task Success()
        {
            //Arrange
            ExceptionFilterAttribute attribute = new();
            //Act
            //Assert
            Assert.IsType<ForbidResult>(await GetResultAsync(attribute, new NoPermissionsException()));
            Assert.IsType<BadRequestObjectResult>(await GetResultAsync(attribute, new EntityNotFoundException("kinda id")));
            Assert.IsType<BadRequestResult>(await GetResultAsync(attribute, new ArgumentException()));
            Assert.IsType<BadRequestResult>(await GetResultAsync(attribute, new InvalidOperationException()));
            await Assert.ThrowsAsync<Exception>(async () => await GetResultAsync(attribute, new Exception()));
        }
    }
}