using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        /// <param name="request">Request parameter example</param>
        /// <returns>Information for testing authorization.</returns>
        /// <response code="200">Returns the test information for authorization.</response>
        /// <response code="401">The client must authenticate itself to get the requested response. The client is not authorized to access the resource.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TestDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<TestDto> Get(TestRequest request)
        {
            return Ok(new TestDto { UserName = User.Identity?.Name, Number = request.RequiredNumber });
        }

        /// <summary>
        /// Data model for testing authorization.
        /// 
        /// </summary>
        public record TestDto
        {
            /// <summary>
            /// User name.
            /// </summary>
            [DefaultValue("User Name")]
            public string? UserName { get; set; } = "Default Name";

            /// <summary>
            /// Random number
            /// </summary>
            [DefaultValue(1)]
            public int Number { get; set; } = 1;
        }


        /// <summary>
        /// Test Authorization request
        /// </summary>
        public record TestRequest
        {
            /// <summary>
            /// Not required info
            /// </summary>
            [DefaultValue("Info")]
            public string? NotRequiredInfo { get; set; }

            /// <summary>
            /// Required info
            /// </summary>
            [Required]
            [DefaultValue(10)]
            public int RequiredNumber { get; set; }
        }
    }
}
