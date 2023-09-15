using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.HubClients.PrivateChats.PrivateChatCreated;
using Sparkle.Application.Users.Commands.AcceptFriendRequest;
using Sparkle.Application.Users.Commands.FriendRequest;
using Sparkle.Application.Users.Queries.GetRelationships;
using Sparkle.Application.Users.Queries.GetUserDetails;

namespace Sparkle.WebApi.Controllers
{
    [Route("api/users")]
    public class UsersController : ApiControllerBase
    {
        public UsersController(IMediator mediator, IAuthorizedUserProvider userProvider) : base(mediator, userProvider)
        {
        }

        /// <summary>
        /// Gets detailed information about the provided user, including it's ServerProfile if ServerId is provided
        /// </summary>
        /// <param name="userId">Id of requested user. If null will return current user</param>
        /// <param name="serverId">string ObjectId represents of server. Can be provided if ServerProfile is required. Null by default</param>
        /// <returns>UserDetails object</returns>
        /// <response code="200">Ok. User details object in JSON</response>
        /// <response code="400">Bad Request. The requested user is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetUserDetailsDto>> GetUser(Guid? userId = null, string? serverId = null)
        {
            GetUserDetailsQuery query = new()
            {
                UserId = userId ?? UserId,
                ServerId = serverId
            };
            GetUserDetailsDto user = await Mediator.Send(query);

            return Ok(user);
        }

        /// <summary>
        /// Gets all relationships of the current user
        /// </summary>
        /// <response code="200">Ok. List of current user relationships in JSON</response>
        /// <response code="400">Bad Request. The requested user is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <returns>List of current user relationships</returns>
        [HttpGet("relationships")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<RelationshipDto>>> GetRelationships()
        {
            return Ok(await Mediator.Send(new GetRelationshipQuery()));
        }

        /// <summary>
        /// Sends a friend request to the user with the provided id
        /// </summary>
        /// <param name="userId">Id of the user to send a friend request to</param>
        /// <response code="204">No Content. The request was sent successfully</response>
        /// <response code="400">Bad Request. The requested user is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client is blocked by the requested user</response>
        [HttpPost("add-friend")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> SendFriendRequest(Guid userId)
        {
            CreateFriendRequestCommand request = new() { UserId = userId };
            string? newChat = await Mediator.Send(request);

            if (newChat != null)
                await Mediator.Send(new NotifyPrivateChatCreatedQuery { ChatId = newChat });

            return NoContent();
        }

        /// <summary>
        /// Accepts a friend request from the user with the provided id
        /// </summary>
        /// <param name="userId">Id of the user to accept a friend request from</param>
        /// <response code="204">No Content. The request was accepted successfully</response>
        /// <response code="400">Bad Request. The requested user is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has no friend request from the requested user</response>
        [HttpPost("accept-friend")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> AcceptFriendRequest(Guid userId)
        {
            AcceptFriendRequestCommand command = new() { UserId = userId };
            await Mediator.Send(command);

            return NoContent();
        }
    }
}