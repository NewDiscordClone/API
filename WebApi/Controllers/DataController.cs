using Application.Exceptions;
using Application.Queries.GetMessages;
using Application.Queries.GetPrivateChats;
using Application.Queries.GetUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Providers;

namespace WebApi.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    [Authorize]
    public class DataController : ApiControllerBase
    {
        public DataController(IMediator mediator, IAuthorizedUserProvider userProvider) : base(mediator, userProvider)
        {
        }

            // int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
            //                            throw new NoSuchUserException());

        

        [HttpPost]
        public async Task<ActionResult<GetUserDetailsDto>> GetUser([FromBody] GetUserDetailsRequest getUserDetailsRequest)
        {
            GetUserDetailsDto user = await Mediator.Send(getUserDetailsRequest);
            return Ok(user);
        }
        [HttpGet]
        public async Task<ActionResult<GetUserDetailsDto>> GetCurrentUser()
        {
            GetUserDetailsDto user = await Mediator
                .Send(new GetUserDetailsRequest { UserId = UserId });
            return Ok(user);
        }
    }
}