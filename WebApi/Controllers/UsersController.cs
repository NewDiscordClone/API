using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.HubClients.Users.RelationshipDeleted;
using Sparkle.Application.HubClients.Users.RelationshipUpdated;
using Sparkle.Application.Models;
using Sparkle.Application.Users.Commands.SendMessageToUser;
using Sparkle.Application.Users.Queries.GetRelationships;
using Sparkle.Application.Users.Queries.GetUserByUserName;
using Sparkle.Application.Users.Queries.GetUserDetails;
using Sparkle.Application.Users.Relationships.AcceptFriendRequest;
using Sparkle.Application.Users.Relationships.CancelFriendRequest;
using Sparkle.Application.Users.Relationships.DeleteFriend;
using Sparkle.Application.Users.Relationships.SendFriendRequest;

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
        /// <param name="userName">UserName of requested user.</param>
        /// <param name="serverId">string ObjectId represents of server. Can be provided if ServerProfile is required. Null by default</param>
        /// <returns>UserDetails object</returns>
        /// <response code="200">Ok. User details object in JSON</response>
        /// <response code="400">Bad Request. The requested user is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetUserDetailsDto>> GetUser(Guid? userId = null, string? userName = null, string? serverId = null)
        {
            GetUserDetailsDto user;
            if (userName == null)
            {
                GetUserDetailsQuery query = new()
                {
                    UserId = userId ?? UserId,
                    ServerId = serverId
                };
                user = await Mediator.Send(query);
            }
            else
            {
                GetUserByUserNameQuery query = new()
                {
                    UserName = userName,
                    ServerId = serverId
                };
                user = await Mediator.Send(query);
            }
            return Ok(user);
        }


        /// <param name="request">
        ///
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> SendMessageToUser(SendMessageToUserRequest request)
        {
            return Problem(statusCode: StatusCodes.Status501NotImplemented);

            MessageChatDto messageChat = await Mediator.Send(request);
            //await Mediator.Send(new NotifyPrivateChatSavedQuery { ChatId = messageChat.ChatId });
            //await Mediator.Send(new NotifyMessageAddedQuery() { MessageId = messageChat.MessageId });
            //await Mediator.Send(new NotifyRelationshipUpdatedRequest() { UserId = UserId });
            //await Mediator.Send(new NotifyRelationshipUpdatedRequest() { UserId = request.UserId });
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
        public async Task<ActionResult> GetRelationships()
        {
            return Ok(await Mediator.Send(new GetRelationshipQuery()));
        }

        /// <summary>
        /// Sends a friend request to the user with the provided id
        /// </summary>
        /// <param name="friendId">Id of the user to send a friend request to</param>
        /// <response code="204">No Content. The request was sent successfully</response>
        /// <response code="400">Bad Request. The requested user is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client is blocked by the requested user</response>
        [HttpPost("friends")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> SendFriendRequest(Guid friendId)
        {
            CreateFriendRequestCommand request = new() { FriendId = friendId };
            Relationship relationship = await Mediator.Send(request);

            await Mediator.Send(new NotifyRelationshipUpdatedQuery() { Relationship = relationship });

            return NoContent();
        }

        /// <summary>
        /// Accepts a friend request from the user with the provided id
        /// </summary>
        /// <param name="friendId">Id of the user to accept a friend request from</param>
        /// <response code="204">No Content. The request was accepted successfully</response>
        /// <response code="400">Bad Request. The requested user is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has no friend request from the requested user</response>
        [HttpPatch("friends")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> AcceptFriendRequest(Guid friendId)
        {
            AcceptFriendRequestCommand command = new() { FriendId = friendId };
            Relationship relationship = await Mediator.Send(command);

            await Mediator.Send(new NotifyRelationshipUpdatedQuery() { Relationship = relationship });

            return NoContent();
        }

        [HttpDelete("friends/cancel")]
        public async Task<ActionResult> CancelFriendRequest(Guid friendId)
        {
            CancelFriendRequestCommand command = new() { FriendId = friendId };
            Relationship relationship = await Mediator.Send(command);

            await Mediator.Send(new NotifyRelationshipDelatedQuery() { Relationship = relationship });

            return NoContent();
        }

        [HttpDelete("friends")]
        public async Task<ActionResult> DeleteFriendRequest(Guid friendId)
        {
            DeleteFriendCommand command = new() { FriendId = friendId };
            Relationship relationship = await Mediator.Send(command);

            await Mediator.Send(new NotifyRelationshipDelatedQuery() { Relationship = relationship });

            return NoContent();
        }
    }
}