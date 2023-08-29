using Application.Providers;
using Application.Queries.GetUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    [Authorize]
    public class DataController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IAuthorizedUserProvider _userProvider;

        public DataController(IMediator mediator, IAuthorizedUserProvider userProvider)
        {
            _mediator = mediator;
            _userProvider = userProvider;
        }

        protected int UserId => _userProvider.GetUserId();
        // int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
        //                            throw new NoSuchUserException());



        [HttpPost]
        public async Task<ActionResult<GetUserDetailsDto>> GetUser([FromBody] GetUserDetailsRequest getUserDetailsRequest)
        {
            GetUserDetailsDto user = await _mediator.Send(getUserDetailsRequest);
            return Ok(user);
        }
        [HttpGet]
        public async Task<ActionResult<GetUserDetailsDto>> GetCurrentUser()
        {
            GetUserDetailsDto user = await _mediator
                .Send(new GetUserDetailsRequest { UserId = UserId });
            return Ok(user);
        }
    }
}