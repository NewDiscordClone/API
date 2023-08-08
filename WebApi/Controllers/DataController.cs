using Application.Models;
using Application.RequestModels;
using Application.RequestModels.GetMessages;
using Application.RequestModels.GetPrivateChats;
using Application.RequestModels.GetServer;
using Application.RequestModels.GetUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Exceptions;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class DataController : Controller
    {
        private readonly IMediator _mediator;

        protected int UserId => int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                       throw new NoSuchUserException());

        [HttpGet]
        public async Task<ActionResult<List<PrivateChat>>> GetPrivateChats()
        {

            GetPrivateChatsRequest get = new() { UserId = UserId };
            List<GetPrivateChatLookUpDto> list = await _mediator.Send(get);
            return Ok(list);
        }

        [HttpGet]
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

        [HttpGet]
        public async Task<ActionResult<List<GetServerLookupDto>>> GetServers()
        {
            GetServersRequest get = new() { UserId = UserId };
            List<GetServerLookupDto> servers = await _mediator.Send(get);
            return Ok(servers);
        }

        [HttpGet]
        public async Task<ActionResult<List<GetMessageLookUpDto>>> GetMessages([FromBody] GetMessagesRequest get)
        {
            List<GetMessageLookUpDto> messages = await _mediator.Send(get);
            return Ok(messages);
        }
    }
}