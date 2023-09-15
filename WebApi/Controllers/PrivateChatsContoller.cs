using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.GroupChats.Commands.AddMemberToGroupChat;
using Sparkle.Application.GroupChats.Commands.ChangeGroupChatImage;
using Sparkle.Application.GroupChats.Commands.CreateGroupChat;
using Sparkle.Application.GroupChats.Commands.LeaveFromGroupChat;
using Sparkle.Application.GroupChats.Commands.MakeGroupChatOwner;
using Sparkle.Application.GroupChats.Commands.RemoveGroupChatMember;
using Sparkle.Application.GroupChats.Commands.RenameGroupChat;
using Sparkle.Application.GroupChats.Queries.GetGroupChatDetails;
using Sparkle.Application.GroupChats.Queries.GetPrivateChats;
using Sparkle.Application.HubClients.PrivateChats.PrivateChatCreated;
using Sparkle.Application.HubClients.PrivateChats.PrivateChatUpdated;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;
using Sparkle.Contracts.PrivateChats;

namespace Sparkle.WebApi.Controllers
{
    [Route("api/private-chats")]
    public class PrivateChatsController : ApiControllerBase
    {
        public PrivateChatsController(IMediator mediator, IAuthorizedUserProvider userProvider, IMapper mapper)
            : base(mediator, userProvider, mapper)
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
            GetPrivateChatsQuery get = new();
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
        [HttpGet("{chatId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<GroupChat>> GetGroupChatDetails(string chatId)
        {
            GroupChat chat = await Mediator
                .Send(new GetGroupChatDetailsQuery() { ChatId = chatId });

            return Ok(chat);
        }

        /// <summary>
        /// Creates new group chat
        /// </summary>
        /// <param name="command">
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
        public async Task<ActionResult<string>> CreateGroupChat(CreateGroupChatCommand command)
        {
            string chatId = await Mediator.Send(command);

            await Mediator.Send(new NotifyPrivateChatCreatedQuery { ChatId = chatId });

            return CreatedAtAction(nameof(GetGroupChatDetails), chatId, chatId);
        }

        /// <summary>
        /// Adds the given user to the group chat as a new member
        /// </summary>
        /// <param name="chatId">Chat Id to add new member to</param>
        /// <param name="userId">Id of the user to add</param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested group chat or member is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPatch("{chatId}/add-member")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> AddMemberToGroupChat(string chatId, Guid userId)
        {
            AddMemberToGroupChatCommand command = new()
            {
                ChatId = chatId,
                NewMemberId = userId
            };
            await Mediator.Send(command);

            await Mediator.Send(new NotifyPrivateChatUpdatedQuery { ChatId = chatId });

            return NoContent();
        }

        /// <summary>
        /// Changes image of the given group chat
        /// </summary>
        /// <param name="chatId">Chat Id to change image for</param>
        /// <param name="imageUrl">URL to the new image</param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested group chat is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPatch("{chatId}/image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> ChangeGroupChatImage(string chatId, [FromBody] string imageUrl)
        {
            ChangeGroupChatImageCommand command = new()
            {
                ChatId = chatId,
                NewImage = imageUrl
            };
            await Mediator.Send(command);

            await Mediator.Send(new NotifyPrivateChatUpdatedQuery { ChatId = chatId });

            return NoContent();
        }

        /// <summary>
        /// Changes the title of the given group chat
        /// </summary>
        /// <param name="chatId">Chat Id to change title for</param>
        /// <param name="name">New title of the group chat</param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested group chat is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPatch("{chatId}/name")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RenameGroupChat(string chatId, string name)
        {
            RenameGroupChatCommand command = new()
            {
                ChatId = chatId,
                NewTitle = name
            };
            await Mediator.Send(command);

            await Mediator.Send(new NotifyPrivateChatUpdatedQuery { ChatId = chatId });

            return NoContent();
        }

        /// <summary>
        /// Remove the currently authorized user from the group chat 
        /// </summary>
        /// <param name="chatId">Chat Id to leave from</param>
        /// <param name="silent">By default false; if true, the other chat members will not be notified</param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested group chat is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client must be a member of the chat</response>
        [HttpDelete("{chatId}/leave")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> LeaveFromGroupChat(string chatId, bool silent = false)
        {
            LeaveFromGroupChatCommand command = new()
            {
                ChatId = chatId,
                Silent = silent
            };
            await Mediator.Send(command);

            await Mediator.Send(new NotifyPrivateChatUpdatedQuery { ChatId = chatId });

            return NoContent();
        }

        /// <summary>
        /// Transfer owner rights of the group chat to another member of the chat
        /// </summary>
        /// <param name="chatId">Chat Id to change owner for</param>
        /// <param name="newOwnerId">Id of the new owner</param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested group chat or user is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this command</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPatch("{chatId}/change-owner")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> ChangeGroupChatOwner(string chatId, Guid newOwnerId)
        {
            ChangeGroupChatOwnerCommand command = new()
            {
                ChatId = chatId,
                MemberId = newOwnerId
            };
            await Mediator.Send(command);

            await Mediator.Send(new NotifyPrivateChatUpdatedQuery { ChatId = chatId });

            return NoContent();
        }

        /// <summary>
        /// Removes the given user from the chat members list
        /// </summary>
        /// <param name="chatId">Chat Id to remove member from</param>
        /// <param name="request">
        /// ```
        /// memberId: int // Id of the user to remove
        /// silent: boolean // by default false; if true, the other chat members will not be notified
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested group chat or user is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpDelete("{chatId}/kick")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RemoveGroupChatMember(string chatId, KickUserFromGroupChatRequest request)
        {
            RemoveGroupChatMemberCommand command = Mapper.Map<RemoveGroupChatMemberCommand>((chatId, request));
            await Mediator.Send(command);

            await Mediator.Send(new NotifyPrivateChatUpdatedQuery { ChatId = chatId });

            return NoContent();
        }
    }
}