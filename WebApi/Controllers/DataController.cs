using System.Security.Claims;
using Application.Models;
using Application.RequestModels;
using Application.RequestModels.GetMessages;
using Application.RequestModels.GetPrivateChats;
using Application.RequestModels.GetServer;
using Application.RequestModels.GetUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    // [Authorize]
    public class DataController : Controller
    {
        private readonly IMediator _mediator;

        [HttpGet]
        public async Task<ActionResult<List<PrivateChat>>> GetPrivateChats()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                       throw new NoSuchUserException());
                GetPrivateChatsRequest get = new() { UserId = userId };
                var list = await _mediator.Send(get);
                return Ok(list);
            }
            catch (NoSuchUserException)
            {
                return Unauthorized("User not found");
            }
        }

        [HttpGet]
        public async Task<ActionResult<GetUserDto>> GetUser()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                       throw new NoSuchUserException());
                GetUserRequest get = new() { UserId = userId };
                var user = await _mediator.Send(get);
                return Ok(user);
            }
            catch (NoSuchUserException)
            {
                return Unauthorized("User not found");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<GetServerDto>>> GetServers()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                       throw new NoSuchUserException());
                GetServersRequest get = new() { UserId = userId };
                var servers = await _mediator.Send(get);
                return Ok(servers);
            }
            catch (NoSuchUserException)
            {
                return Unauthorized("User not found");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<GetServerDto>>> GetMessages([FromBody] GetMessagesRequest get)
        {
            var messages = await _mediator.Send(get);
            return Ok(messages);
        }
    }
}