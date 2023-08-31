using System.ComponentModel;
using Application.Commands.Channels.CreateChannel;
using Application.Commands.Channels.RemoveChannel;
using Application.Commands.Channels.RenameChannel;
using Application.Common.Exceptions;
using Application.Models;
using Application.Providers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Attributes;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ExceptionFilter]
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
            Channel chat = await Mediator.Send(request);
            //TODO: Реалізація відправки Notify
            return Created("", chat.Id);
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