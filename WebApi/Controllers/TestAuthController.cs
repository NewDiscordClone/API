using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    /// <summary>
    /// Controller for testing authorization.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TestAuthController : ControllerBase
    {
        /// <summary>
        /// Get information for testing authorization.
        /// </summary>
        /// <remarks>
        /// This method provides information for testing authorization.
        /// To successfully execute the request, the client must be authenticated.
        /// </remarks>
        /// <returns>Information for testing authorization.</returns>
        /// <response code="200">Returns the test information for authorization.</response>
        /// <response code="401">The client must authenticate itself to get the requested response. The client is not authorized to access the resource.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TestDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<TestDto> Get()
        {
            return Ok(new TestDto { UserName = User.Identity?.Name, Number = 5 });
        }

        /// <summary>
        /// Data model for testing authorization.
        /// </summary>
        public record TestDto
        {
            /// <summary>
            /// User name.
            /// </summary>
            public string? UserName { get; set; } = "Default Name";

            /// <summary>
            /// Number for testing.
            /// </summary>
            public int Number { get; set; } = 1;
        }
    }
}
