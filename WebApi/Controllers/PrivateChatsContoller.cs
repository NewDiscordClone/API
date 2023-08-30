using Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Commands.PrivateChats.AddMemberToPrivateChat;
using Application.Commands.PrivateChats.ChangePrivateChatImage;
using Application.Commands.PrivateChats.CreatePrivateChat;
using Application.Commands.PrivateChats.LeaveFromPrivateChat;
using Application.Commands.PrivateChats.MakePrivateChatOwner;
using Application.Commands.PrivateChats.RemovePrivateChatMember;
using Application.Commands.PrivateChats.RenamePrivateChat;
using Application.Models;
using Application.Interfaces;
using Application.Queries.GetPrivateChatDetails;
using Application.Queries.GetPrivateChats;
using MongoDB.Bson;

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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        }

        /// <summary>
        /// Creates new private chat
        /// </summary>
        /// <param name="request"> Create private chat model
        /// ```
        /// title: string // up to 100 characters
        /// image?: string // URL to the image media file
        /// usersId: number[] // users that are members of the chat from the beginning
        /// ```
        /// </param>
        /// <returns>string representation of an ObjectId of a newly created private chat</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> CreatePrivateChat(CreatePrivateChatRequest request)
        {
            PrivateChat chat = await Mediator.Send(request);
            //TODO: Реалізація відправки Notify
            return Created("https://localhost:7060/api/PrivateChat/GetDetails?chatId=" + chat.Id, chat.Id);
        }

        /// <summary>
        /// Adds the given user to the private chat as a new member
        /// </summary>
        /// <param name="request"> New member model
        /// ```
        /// chatId: string // represents ObjectId of the chat to add new member to
        /// newMemberId: int // Id of the user to add
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        }

        /// <summary>
        /// Changes image of the given private chat
        /// </summary>
        /// <param name="request"> Change chat image model
        /// ```
        /// chatId: string // represents ObjectId of the chat to change image
        /// newImage: string // URL to the image media file
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> ChangePrivateChatImage(ChangePrivateChatImageRequest request)
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
        /// Changes the title of the given private chat
        /// </summary>
        /// <param name="request">
        /// ```
        /// chatId: string // represents ObjectId of the chat to rename
        /// newTitle: string // up to 100 characters
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RenamePrivateChat(RenamePrivateChatRequest request)
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
        /// Remove the currently authorized user from the private chat 
        /// </summary>
        /// <param name="request"> Chat Id model
        /// ```
        /// chatId: string // represents ObjectId of the chat to leave from
        /// silent: boolean // by default false; if true, the other chat members will not be notified 
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> LeaveFromPrivateChat(LeaveFromPrivateChatRequest request)
        {
            try
            {
                await Mediator.Send(request);
                //TODO: Реалізація відправки Notify
                return Ok();
            }
            catch (NoSuchUserException e)
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
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> MakePrivateChatOwner(MakePrivateChatOwnerRequest request)
        {
            try
            {
                await Mediator.Send(request);
                return Ok();
            }
            catch (NoPermissionsException e)
            {
                return Forbid(e.Message);
            }
        }

        /// <summary>
        /// Removes the given user from the chat members list
        /// </summary>
        /// <param name="request">
        /// ```
        /// chatId: string // represents ObjectId of the chat to remove new member from
        /// memberId: int // Id of the user to remove
        /// silent: boolean //by default false; if true, the other chat members will not be notified
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RemovePrivateChatMember(RemovePrivateChatMemberRequest request)
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
            catch (NoSuchUserException ex)
            {
                return BadRequest(ex);
            }
        }
    }
}