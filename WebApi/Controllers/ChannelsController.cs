using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Channels.Commands.CreateChannel;
using Sparkle.Application.Channels.Commands.RemoveChannel;
using Sparkle.Application.Channels.Commands.RenameChannel;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.HubClients.Channels.ChannelCreated;
using Sparkle.Application.HubClients.Channels.ChannelRemoved;
using Sparkle.Application.HubClients.Channels.ChannelUpdated;
using Sparkle.Application.Models;

namespace Sparkle.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ChannelsController : ApiControllerBase
    {
        public ChannelsController(IMediator mediator, IAuthorizedUserProvider userProvider) : base(mediator, userProvider)
        {
        }

        /// <summary>
        /// Create a text channel attached to a server
        /// </summary>
        /// <param name="request">
        /// ```
        /// title: string // up to 100 characters
        /// serverId: string // represents ObjectId
        /// ```
        /// </param>
        /// <returns>
        /// String that represents ObjectId of the created Channel instance
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> CreateChannel([FromBody] CreateChannelRequest request)
        {
            string chatId = await Mediator.Send(request);
            await Mediator.Send(new NotifyChannelCreatedRequest { ChannelId = chatId });
            return Created("", chatId);
        }

        /// <summary>
        /// A request to set a new title for a provided channel
        /// </summary>
        /// <param name="request">
        /// ```
        /// chatId: string // represents ObjectId
        /// newTitle: string // up to 100 characters
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="200">Ok. Operation is successful</response>
        /// <response code="400">Bad Request. The requested channel is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RenameChannel(RenameChannelRequest request)
        {
            try
            {
                await Mediator.Send(request);
                await Mediator.Send(new NotifyChannelUpdatedRequest() { ChannelId = request.ChatId });
                return Ok();
            }
            catch (NoPermissionsException e)
            {
                return Forbid(e.Message);
            }
        }
        /// <summary>
        /// A request to remove the provided channel by it's id
        /// </summary>
        /// <param name="request">
        /// ```
        /// chatId: string // represents ObjectId of a channel
        /// ```
        /// </param>
        /// <returns>Ok the if operation is successful</returns>
        /// <response code="200">Ok. Operation is successful</response>
        /// <response code="400">Bad Request. The requested channel is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RemoveChannel(RemoveChannelRequest request)
        {
            try
            {
                Channel channel = await Mediator.Send(request);
                await Mediator.Send(new NotifyChannelRemovedRequest() { Channel = channel });
                return Ok();
            }
            catch (NoPermissionsException e)
            {
                return Forbid(e.Message);
            }
        }
    }
}