using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Sparkle.Application.Common.Exceptions;
using ExceptionFilterAttribute = Sparkle.WebApi.Attributes.ExceptionFilterAttribute;

namespace Sparkle.Tests
{
    public class ExceptionFilterTests
    {
        private readonly ActionContext _actionContext = new()
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };

        private async Task<IActionResult?> GetResultAsync(ExceptionFilterAttribute attribute, Exception exception)
        {
            ExceptionContext context = new(_actionContext, new List<IFilterMetadata>())
            {
                Exception = exception,
            };
            await attribute.OnExceptionAsync(context);
            return context.Result;
        }

        private static void CheckProblemDetailsResult(
            IActionResult? result,
            int expectedStatusCode,
            string expectedTitle)
        {
            Assert.IsType<ObjectResult>(result);
            Assert.IsType<ProblemDetails>((result as ObjectResult)?.Value);
            ProblemDetails? problem = (result as ObjectResult)!.Value! as ProblemDetails;
            Assert.Equal(expectedStatusCode, problem?.Status);
            Assert.Equal(expectedTitle, problem?.Title);
        }

        [Fact]
        public async Task NoPermissionsException_Returns403Forbidden()
        {
            // Arrange
            ExceptionFilterAttribute attribute = new();
            NoPermissionsException exception = new("Permission denied");

            // Act
            IActionResult? result = await GetResultAsync(attribute, exception);

            // Assert
            CheckProblemDetailsResult(result, StatusCodes.Status403Forbidden, "Permission denied");
        }

        [Fact]
        public async Task EntityNotFoundException_Returns404NotFound()
        {
            // Arrange
            ExceptionFilterAttribute attribute = new();
            EntityNotFoundException exception = new("kinda id");

            // Act
            IActionResult? result = await GetResultAsync(attribute, exception);

            // Assert
            CheckProblemDetailsResult(result, StatusCodes.Status404NotFound, "Entity kinda id not found");
        }

        [Fact]
        public async Task ArgumentException_Returns400BadRequest()
        {
            // Arrange
            ExceptionFilterAttribute attribute = new();
            ArgumentException exception = new("Invalid argument");

            // Act
            IActionResult? result = await GetResultAsync(attribute, exception);

            // Assert
            CheckProblemDetailsResult(result, StatusCodes.Status400BadRequest, "Invalid argument");
        }

        [Fact]
        public async Task InvalidOperationException_Returns400BadRequest()
        {
            // Arrange
            ExceptionFilterAttribute attribute = new();
            InvalidOperationException exception = new("Invalid operation");

            // Act
            IActionResult? result = await GetResultAsync(attribute, exception);

            // Assert
            CheckProblemDetailsResult(result, StatusCodes.Status400BadRequest, "Invalid operation");
        }

        [Fact]
        public async Task Exception_Returns500InternalServerError()
        {
            // Arrange
            ExceptionFilterAttribute attribute = new();
            Exception exception = new("Internal error");

            // Act
            IActionResult? result = await GetResultAsync(attribute, exception);

            // Assert
            CheckProblemDetailsResult(result, StatusCodes.Status500InternalServerError, exception.Message);
        }
    }
}
