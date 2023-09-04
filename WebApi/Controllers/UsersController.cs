#nullable enable
using Application.Commands.HubClients.Messages.MessageAdded;
using Application.Commands.HubClients.PrivateChats.PrivateChatCreated;
using Application.Commands.Users.AcceptFriendRequest;
using Application.Commands.Users.FriendRequest;
using Application.Commands.Users.SendMessageToUser;
using Application.Common.Exceptions;
using Application.Models;
using Application.Providers;
using Application.Queries.GetRelationships;
using Application.Queries.GetUser;
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
    public class UsersController : ApiControllerBase
    {
        public UsersController(IMediator mediator, IAuthorizedUserProvider userProvider) : base(mediator, userProvider)
        {
        }

        /// <summary>
        /// Gets detailed information about the provided user, including it's ServerProfile if ServerId is provided
        /// </summary>
        /// <param name="userId"> The id (int) of the user to get information from </param>
        /// <param name="serverId">string ObjectId represents of server. Can be provided if ServerProfile is required. Null by default</param>
        /// <returns>UserDetails object</returns>
        /// <response code="200">Ok. User details object in JSON</response>
        /// <response code="400">Bad Request. The requested user is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetUserDetailsDto>> GetUser(
            [FromQuery] int userId, [FromQuery] string serverId = null!)
        {
            try
            {
                GetUserDetailsDto user = await Mediator.Send(new GetUserDetailsRequest
                    { UserId = userId, ServerId = serverId });
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Gets detailed information about the currently authorized user
        /// </summary>
        /// <returns>UserDetails object</returns>
        /// <response code="200">Ok. User details object in JSON</response>
        /// <response code="400">Bad Request. The requested user is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetUserDetailsDto>> GetCurrentUser()
        {
            GetUserDetailsDto user = await Mediator
                .Send(new GetUserDetailsRequest { UserId = UserId });
            return Ok(user);
        }

        /// <summary>
        /// Sends message to user you don't have chat with.
        /// This request will create new chat and set relationship <see cref="RelationshipType.Acquaintance"/> to both
        /// </summary>
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
            try
            {
                MessageChatDto messageChat = await Mediator.Send(request);
                await Mediator.Send(new NotifyPrivateChatCreatedRequest { ChatId = messageChat.ChatId });
                await Mediator.Send(new NotifyMessageAddedRequest { MessageId = messageChat.MessageId });
                return NoContent();
            }
            catch (NoPermissionsException)
            {
                return Forbid();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<RelationshipDto>>> GetRelationships()
        {
            try
            {
                return Ok(await Mediator.Send(new GetRelationshipRequest()));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> SendFriendRequest(FriendRequestRequest request)
        {
            try
            {
                string? newChat = await Mediator.Send(request);
                if (newChat != null) await Mediator.Send(new NotifyPrivateChatCreatedRequest { ChatId = newChat });
                return NoContent();
            }
            catch (NoPermissionsException)
            {
                return Forbid();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> AcceptFriendRequest(AcceptFriendRequestRequest request)
        {
            try
            {
                await Mediator.Send(request);
                return NoContent();
            }
            catch (NoPermissionsException)
            {
                return Forbid();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}