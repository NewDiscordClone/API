using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Channels.Commands.CreateChannel;
using Sparkle.Application.Channels.Commands.RemoveChannel;
using Sparkle.Application.Channels.Commands.RenameChannel;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.Events;
using Sparkle.WebApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.WebApi.Controllers
{

    [Route("api/servers/{serverId}/channels")]
    [ServerAuthorize(Claims = Constants.Claims.ManageChannels)]
    public class ChannelsController(IMediator mediator, IAuthorizedUserProvider userProvider) :
        ApiControllerBase(mediator, userProvider)
    {

        /// <summary>
        /// Create a text channel attached to a server
        /// </summary>
        ///<param name="name">Name of the channel to be created</param>
        ///<param name="serverId">Id of the server to attach the channel to</param>
        /// <returns>
        /// String that represents ObjectId of the created Channel instance
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> CreateChannel(string serverId, [Required] string name)
        {
            CreateChannelCommand command = new()
            {
                ServerId = serverId,
                Title = name
            };
            Channel channel = await Mediator.Send(command);

            await Mediator.Publish(new ChannelCreatedEvent(channel));

            return Created("", channel.Id);
        }

        /// <summary>
        /// A request to set a new title for a provided channel
        /// </summary>
        /// <remarks>
        /// PATCH: api/servers/5f95a3c3d0ddad0017ea9291/channels/5f95a3c3d0ddad0017ea9291/rename?name=NewTitle
        /// </remarks>
        /// <param name="channelId">Id of the channel to be renamed</param>
        /// <param name="name">New name of the channel</param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">Operation is successful</response>
        /// <response code="400">Bad Request. The requested channel is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPatch("{channelId}/rename")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RenameChannel(string channelId, [Required] string name)
        {
            RenameChannelCommand command = new()
            { ChatId = channelId, NewTitle = name };
            Channel channel = await Mediator.Send(command);

            await Mediator.Publish(new ChannelUpdatedEvent(channel));

            return NoContent();
        }

        /// <summary>
        /// A request to remove the provided channel by it's id
        /// </summary>
        /// <param name="channelId"> Id of the channel to be removed </param>
        /// <returns>Ok the if operation is successful</returns>
        /// <response code="204">Operation is successful</response>
        /// <response code="400">Bad Request. The requested channel is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpDelete("{channelId}/delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RemoveChannel(string channelId)
        {
            Channel channel = await Mediator.Send(new RemoveChannelCommand { ChatId = channelId });

            await Mediator.Send(new ChannelRemovedEvent(channel));

            return NoContent();
        }
    }
}