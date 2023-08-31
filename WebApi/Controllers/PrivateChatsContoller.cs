using Application.Commands.PrivateChats.AddMemberToPrivateChat;
using Application.Commands.PrivateChats.ChangePrivateChatImage;
using Application.Commands.PrivateChats.CreatePrivateChat;
using Application.Commands.PrivateChats.LeaveFromPrivateChat;
using Application.Commands.PrivateChats.MakePrivateChatOwner;
using Application.Commands.PrivateChats.RemovePrivateChatMember;
using Application.Commands.PrivateChats.RenamePrivateChat;
using Application.Common.Exceptions;
using Application.Models;
using Application.Providers;
using Application.Queries.GetPrivateChatDetails;
using Application.Queries.GetPrivateChats;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
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
        /// <response code="200">Ok. List of the private chats</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<PrivateChat>>> GetAllPrivateChats()
        {
            GetPrivateChatsRequest get = new();
            List<PrivateChat> list = await Mediator.Send(get);
            return Ok(list);
        }

        /// <summary>
        /// Get details about the given private chat. The details include Title, Image, OwnerId and Users
        /// </summary>
        /// <param name="chatId">Chat Id to get detailed information from</param>
        /// <returns>Json private chat object</returns>
        /// <response code="200">Ok. Json private chat object</response>
        /// <response code="400">Bad Request. The requested private chat is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PrivateChat>> GetPrivateChatDetails(string chatId)
        {
            try
            {
                PrivateChat chat = await Mediator
                    .Send(new GetPrivateChatDetailsRequest() { ChatId = chatId });
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
        /// Creates new private chat
        /// </summary>
        /// <param name="request">
        /// ```
        /// title: string // up to 100 characters
        /// image?: string // URL to the image media file
        /// usersId: number[] // users that are members of the chat from the beginning
        /// ```
        /// </param>
        /// <returns>String representation of an ObjectId of a newly created private chat</returns>
        /// <response code="201">Created. String representation of an ObjectId of a newly created private chat</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> CreatePrivateChat(CreatePrivateChatRequest request)
        {
            PrivateChat chat = await Mediator.Send(request);
            //TODO: Реалізація відправки Notify
            return Created($"{this.Request.Scheme}://{this.Request.Host}/api/PrivateChat/GetDetails?chatId=" + chat.Id, chat.Id);
        }

        /// <summary>
        /// Adds the given user to the private chat as a new member
        /// </summary>
        /// <param name="request">
        /// ```
        /// chatId: string // represents ObjectId of the chat to add new member to
        /// newMemberId: int // Id of the user to add
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested private chat or member is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> AddMemberToPrivateChat(AddMemberToPrivateChatRequest request)
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
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Changes image of the given private chat
        /// </summary>
        /// <param name="request">
        /// ```
        /// chatId: string // represents ObjectId of the chat to change image
        /// newImage: string // URL to the image media file
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested private chat is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> ChangePrivateChatImage(ChangePrivateChatImageRequest request)
        {
            try
            {
                await Mediator.Send(request);
                //TODO: Реалізація відправки Notify
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
        /// Changes the title of the given private chat
        /// </summary>
        /// <param name="request">
        /// ```
        /// chatId: string // represents ObjectId of the chat to rename
        /// newTitle: string // up to 100 characters
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested private chat is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RenamePrivateChat(RenamePrivateChatRequest request)
        {
            try
            {
                await Mediator.Send(request);
                //TODO: Реалізація відправки Notify
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
        /// Remove the currently authorized user from the private chat 
        /// </summary>
        /// <param name="request">
        /// ```
        /// chatId: string // represents ObjectId of the chat to leave from
        /// silent: boolean // by default false; if true, the other chat members will not be notified 
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested private chat is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client must be a member of the chat</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> LeaveFromPrivateChat(LeaveFromPrivateChatRequest request)
        {
            try
            {
                await Mediator.Send(request);
                //TODO: Реалізація відправки Notify
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
        /// Transfer owner rights of the private chat to another member of the chat
        /// </summary>
        /// <param name="request">
        /// ```
        /// chatId: string // represents ObjectId of the chat to transfer owner of
        /// memberId: int // id of the user to transfer rights to
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested private chat or user is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> MakePrivateChatOwner(MakePrivateChatOwnerRequest request)
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
        /// <response code="400">Bad Request. The requested private chat or user is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RemovePrivateChatMember(RemovePrivateChatMemberRequest request)
        {
            try
            {
                await Mediator.Send(request);
                //TODO: Реалізація відправки Notify
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