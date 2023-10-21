using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Servers.Roles.Queries.GetClaims;

namespace Sparkle.WebApi.Controllers
{
    [Route("api/claims")]
    public class ClaimsController : ApiControllerBase
    {
        public ClaimsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetClaims()
        {
            IEnumerable<string> claims = await Mediator.Send(new GetClaimsQuery());
            return Ok(claims);
        }
    }
}
