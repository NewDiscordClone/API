using Application.Commands.Channels.CreateChannel;
using Application.Commands.Channels.RemoveChannel;
using Application.Commands.Channels.RenameChannel;
using Application.Exceptions;
using Application.Models;
using Application.Providers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ChannelsController : ApiControllerBase
    {
        public ChannelsController(IMediator mediator, IAuthorizedUserProvider userProvider) : base(mediator, userProvider)
        {
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<int>> CreateChannel([FromBody] CreateChannelRequest request)
        {
            Channel chat = await Mediator.Send(request);
            //TODO: Реалізація відправки Notify
            return Created("https://localhost:7060/api/PrivateChat/GetDetails?chatId="+chat.Id, chat.Id);
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RenameChannel([FromBody] RenameChannelRequest request)
        {
            try
            {
                await Mediator.Send(request);
                //TODO: Реалізація відправки Notify
                return Ok();
            }
            catch (NoPermissionsException e)
            {
                return Forbid(e.Message);
            }
        }
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RemoveChannel([FromBody] RemoveChannelRequest request)
        {
            try
            {
                await Mediator.Send(request);
                //TODO: Реалізація відправки Notify
                return Ok();
            }
            catch (NoPermissionsException e)
            {
                return Forbid(e.Message);
            }
        }
    }
}