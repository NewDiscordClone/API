using Application.Commands.GroupChats.AddMemberToGroupChat;
using Application.Commands.GroupChats.ChangeGroupChatImage;
using Application.Commands.GroupChats.CreateGroupChat;
using Application.Commands.GroupChats.LeaveFromGroupChat;
using Application.Commands.GroupChats.MakeGroupChatOwner;
using Application.Commands.GroupChats.RemoveGroupChatMember;
using Application.Commands.GroupChats.RenameGroupChat;
using Application.Commands.HubClients.PrivateChats.PrivateChatCreated;
using Application.Commands.HubClients.PrivateChats.PrivateChatUpdated;
using Application.Common.Exceptions;
using Application.Models;
using Application.Models.LookUps;
using Application.Providers;
using Application.Queries.GetGroupChatDetails;
using Application.Queries.GetPrivateChats;
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
    public class PrivateChatsController : ApiControllerBase
    {
        public PrivateChatsController(IMediator mediator, IAuthorizedUserProvider userProvider) : base(mediator,
            userProvider)
        {
        }

        /// <summary>
        /// Gets all Private Chats the currently authorized user are member of
        /// </summary>
        /// <returns>List of the private chats</returns>
        /// <response code="200">Ok. List of the private chat look ups</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<PrivateChatLookUp>>> GetAllPrivateChats()
        {
            GetPrivateChatsRequest get = new();
            List<PrivateChatLookUp> list = await Mediator.Send(get);
            return Ok(list);
        }

        /// <summary>
        /// Get details about the given group chat. The details include Title, Image, OwnerId and Users
        /// </summary>
        /// <param name="chatId">Chat Id to get detailed information from</param>
        /// <returns>Json group chat object</returns>
        /// <response code="200">Ok. Json group chat object</response>
        /// <response code="400">Bad Request. The requested group chat is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<GroupChat>> GetGroupChatDetails(string chatId)
        {
            try
            {
                GroupChat chat = await Mediator
                    .Send(new GetGroupChatDetailsRequest() { ChatId = chatId });
                return Ok(chat);
            }
            catch (NoPermissionsException e)
            {
                return Forbid(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Creates new group chat
        /// </summary>
        /// <param name="request">
        /// ```
        /// title: string // up to 100 characters
        /// image?: string // URL to the image media file
        /// usersId: number[] // users that are members of the chat from the beginning
        /// ```
        /// </param>
        /// <returns>String representation of an ObjectId of a newly created group chat</returns>
        /// <response code="201">Created. String representation of an ObjectId of a newly created group chat</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> CreateGroupChat(CreateGroupChatRequest request)
        {
            string chatId = await Mediator.Send(request);
            await Mediator.Send(new NotifyPrivateChatCreatedRequest { ChatId = chatId });
            return Created($"{Request.Scheme}://{Request.Host}/api/GroupChat/GetDetails?chatId=" + chatId, chatId);
        }

        /// <summary>
        /// Adds the given user to the group chat as a new member
        /// </summary>
        /// <param name="request">
        /// ```
        /// chatId: string // represents ObjectId of the chat to add new member to
        /// newMemberId: int // Id of the user to add
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested group chat or member is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> AddMemberToGroupChat(AddMemberToGroupChatRequest request)
        {
            try
            {
                await Mediator.Send(request);
                await Mediator.Send(new NotifyPrivateChatUpdatedRequest { ChatId = request.ChatId });
                return Ok();
            }
            catch (NoPermissionsException e)
            {
                return Forbid(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Changes image of the given group chat
        /// </summary>
        /// <param name="request">
        /// ```
        /// chatId: string // represents ObjectId of the chat to change image
        /// newImage: string // URL to the image media file
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested group chat is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> ChangeGroupChatImage(ChangeGroupChatImageRequest request)
        {
            try
            {
                await Mediator.Send(request);
                await Mediator.Send(new NotifyPrivateChatUpdatedRequest { ChatId = request.ChatId });
                return NoContent();
            }
            catch (NoPermissionsException e)
            {
                return Forbid(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Changes the title of the given group chat
        /// </summary>
        /// <param name="request">
        /// ```
        /// chatId: string // represents ObjectId of the chat to rename
        /// newTitle: string // up to 100 characters
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested group chat is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RenameGroupChat(RenameGroupChatRequest request)
        {
            try
            {
                await Mediator.Send(request);
                await Mediator.Send(new NotifyPrivateChatUpdatedRequest { ChatId = request.ChatId });
                return NoContent();
            }
            catch (NoPermissionsException e)
            {
                return Forbid(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }
        /// <summary>
        /// Remove the currently authorized user from the group chat 
        /// </summary>
        /// <param name="request">
        /// ```
        /// chatId: string // represents ObjectId of the chat to leave from
        /// silent: boolean // by default false; if true, the other chat members will not be notified 
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested group chat is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client must be a member of the chat</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> LeaveFromGroupChat(LeaveFromGroupChatRequest request)
        {
            try
            {
                await Mediator.Send(request);
                await Mediator.Send(new NotifyPrivateChatUpdatedRequest { ChatId = request.ChatId });
                return NoContent();
            }
            catch (NoSuchUserException e)
            {
                return Forbid(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Transfer owner rights of the group chat to another member of the chat
        /// </summary>
        /// <param name="request">
        /// ```
        /// chatId: string // represents ObjectId of the chat to transfer owner of
        /// memberId: int // id of the user to transfer rights to
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested group chat or user is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> MakeGroupChatOwner(MakeGroupChatOwnerRequest request)
        {
            try
            {
                await Mediator.Send(request);
                return NoContent();
            }
            catch (NoPermissionsException e)
            {
                return Forbid(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Removes the given user from the chat members list
        /// </summary>
        /// <param name="request">
        /// ```
        /// chatId: string // represents ObjectId of the chat to remove new member from
        /// memberId: int // Id of the user to remove
        /// silent: boolean // by default false; if true, the other chat members will not be notified
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested group chat or user is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RemoveGroupChatMember(RemoveGroupChatMemberRequest request)
        {
            try
            {
                await Mediator.Send(request);
                await Mediator.Send(new NotifyPrivateChatUpdatedRequest { ChatId = request.ChatId });
                return NoContent();
            }
            catch (NoPermissionsException e)
            {
                return Forbid(e.Message);
            }
            catch (NoSuchUserException ex)
            {
                return BadRequest(ex);
            }
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}