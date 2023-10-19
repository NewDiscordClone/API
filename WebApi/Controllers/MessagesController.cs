using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.HubClients.Messages.MessageAdded;
using Sparkle.Application.HubClients.Messages.MessageRemoved;
using Sparkle.Application.HubClients.Messages.MessageUpdated;
using Sparkle.Application.Messages.Commands.AddMessage;
using Sparkle.Application.Messages.Commands.AddReaction;
using Sparkle.Application.Messages.Commands.EditMessage;
using Sparkle.Application.Messages.Commands.PinMessage;
using Sparkle.Application.Messages.Commands.RemoveAllReactions;
using Sparkle.Application.Messages.Commands.RemoveAttachment;
using Sparkle.Application.Messages.Commands.RemoveMessage;
using Sparkle.Application.Messages.Commands.RemoveReaction;
using Sparkle.Application.Messages.Commands.UnpinMessage;
using Sparkle.Application.Messages.Queries.GetMessages;
using Sparkle.Application.Messages.Queries.GetPinnedMessages;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;
using Sparkle.Contracts.Messages;
using Sparkle.WebApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.WebApi.Controllers
{
    [Route("api/chats/{chatId}/messages")]
    [ServerAuthorize]
    public class MessagesController : ApiControllerBase
    {
        public MessagesController(IMediator mediator, IAuthorizedUserProvider userProvider, IMapper mapper) : base(mediator,
            userProvider)
        {
            _mapper = mapper;
        }

        private readonly IMapper _mapper;

        /// <summary>
        /// Returns a list of messages to show in the chat
        /// </summary>
        /// <remarks>
        /// The size of a page defined as a constant (see <see cref="GetMessagesQueryHandler"/>)
        /// </remarks>
        /// <param name="chatId">string ObjectId representation of the chat to get pinned messages from</param>
        /// <param name="messagesCount">The amount of messages that already loaded to skip them. Set 0 to load last messages</param>
        /// <param name="onlyPinned">If true, only pinned messages will be returned</param>
        /// <returns>A list of messages to show</returns>
        /// <response code="200">Ok. A list of messages to show</response>
        /// <response code="400">Bad Request. The requested chat is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client must be a member of the chat</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<List<MessageDto>>> GetMessages(string chatId, int messagesCount = 0, bool onlyPinned = false)
        {
            IRequest<List<MessageDto>> query;

            if (onlyPinned)
            {
                query = new GetPinnedMessagesQuery() { ChatId = chatId };
            }
            else
            {
                query = new GetMessagesQuery() { ChatId = chatId, MessagesCount = messagesCount };
            }

            List<MessageDto> messages = await Mediator.Send(query);

            return Ok(messages);
        }

        /// <summary>
        /// Adds message to the given chat and notify other members about it
        /// </summary>
        /// <param name="chatId">string ObjectId representation of the chat to send message to</param>
        /// <param name="request">
        /// ```
        /// text: string // Up to 2000 characters
        /// attachments: Attachment[] // Attachments that user includes to the message
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="201">Created. Message added</response>
        /// <response code="400">Bad Request. The requested chat is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> AddMessage(string chatId, AddMessageRequest request)
        {
            AddMessageCommand command = _mapper.Map<AddMessageCommand>((request, chatId));
            MessageDto message = await Mediator.Send(command);

            await Mediator.Send(new NotifyMessageAddedQuery { MessageDto = message });

            //TODO: Create GetMessage request and send it to the client
            return Created("", message);
        }

        /// <summary>
        /// Adds reaction to the message
        /// </summary>
        /// <param name="messageId">string ObjectId representation of the message to add reaction to</param>
        /// <param name="reaction">Emoji code</param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested message is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client must be a member of the chat</response>
        [HttpPatch("{messageId}/reactions/add")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> AddReaction(string messageId, string reaction)
        {
            AddReactionCommand command = new() { Emoji = reaction, MessageId = messageId };
            await Mediator.Send(command);

            await Mediator.Send(new NotifyMessageUpdatedQuery { MessageId = messageId });

            return NoContent();
        }

        /// <summary>
        /// A request to change the given message text
        /// </summary>
        /// <remarks>
        /// This action can only be performed by the owner of the message
        /// </remarks>
        /// <param name="messageId">string ObjectId representation of the message to edit</param>
        /// <param name="newMessage">New message text</param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested message is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client must be the owner of the message</response>
        [HttpPatch("{messageId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> EditMessage(string messageId, [FromBody] string newMessage)
        {
            EditMessageCommand command = new() { MessageId = messageId, NewText = newMessage };
            await Mediator.Send(command);

            await Mediator.Send(new NotifyMessageUpdatedQuery { MessageId = messageId });

            return NoContent();
        }

        /// <summary>
        /// Pins the selected message to the given chat
        /// </summary>
        /// <remarks>
        /// This action can only be performed by a member of a private chat or a server member with an appropriate role
        /// </remarks>
        /// <param name="messageId">string ObjectId representation of the message to pin</param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested message is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPatch("{messageId}/pin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> PinMessage(string messageId)
        {
            PinMessageCommand command = new() { MessageId = messageId };
            await Mediator.Send(command);

            await Mediator.Send(new NotifyMessageUpdatedQuery { MessageId = messageId });

            return NoContent();
        }

        /// <summary>
        /// Remove all reactions from the given message.
        /// </summary>
        /// <remarks>
        /// This action can only be performed by a server member with an appropriate role
        /// </remarks>
        /// <param name="messageId">string ObjectId representation of the message to remove reactions from</param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested message is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpDelete("{messageId}/reactions/remove-all")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ServerAuthorize(Claims = Constants.Claims.RemoveMessageReactions)]
        public async Task<ActionResult> RemoveAllReactions(string messageId)
        {
            RemoveAllReactionsCommand command = new() { MessageId = messageId };
            await Mediator.Send(command);
            await Mediator.Send(new NotifyMessageUpdatedQuery { MessageId = messageId });

            return NoContent();
        }

        /// <summary>
        /// Remove a selected attachment from the given message
        /// </summary>
        /// <remarks>
        /// This actions can only be performed by the owner of the message
        /// </remarks>
        /// <param name="messageId">string ObjectId representation of the message to remove the attachment from</param>
        /// <param name="attachmentIndex">the index of the attachment to remove</param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested message or attachment is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client must to be the owner of the message</response>
        [HttpDelete("{messageId}/attachments")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RemoveAttachment(string messageId, [Required] int attachmentIndex)
        {
            RemoveAttachmentCommand command = new() { MessageId = messageId, AttachmentIndex = attachmentIndex };
            await Mediator.Send(command);

            await Mediator.Send(new NotifyMessageUpdatedQuery { MessageId = messageId });

            return NoContent();
        }

        /// <summary>
        /// Removes the given message with it's attachments and reactions
        /// </summary>
        /// <remarks>
        /// This action can only be performed by the owner of the message or a server member with an appropriate role
        /// </remarks>
        ///<param name="messageId">string ObjectId representation of the message to remove</param>  
        ///<param name="chatId"></param>  
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested message is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpDelete("{messageId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RemoveMessage(string messageId, string chatId)
        {

            RemoveMessageCommand command = new() { MessageId = messageId, ChatId = chatId };
            Chat chat = await Mediator.Send(command);
            await Mediator.Send(new NotifyMessageRemovedRequest { MessageId = messageId, ChatId = chat.Id });

            return NoContent();
        }

        /// <summary>
        /// Remove the given reaction you have added
        /// </summary>
        /// <remarks>
        /// this action can only be performed by the owner of the reaction
        /// </remarks>
        /// <param name="messageId">string ObjectId representation of the message to remove the reaction from</param>
        /// <param name="reactionIndex">the index of the reaction to remove</param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested message or reaction is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client must to be the owner of the reaction</response>
        [HttpDelete("{messageId}/reactions/remove")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RemoveReaction(string messageId, [Required] int reactionIndex)
        {
            RemoveReactionCommand command = new() { MessageId = messageId, ReactionIndex = reactionIndex };
            await Mediator.Send(command);

            await Mediator.Send(new NotifyMessageUpdatedQuery { MessageId = messageId });

            return NoContent();
        }

        /// <summary>
        /// Unpin previously pinned message
        /// </summary>
        /// <remarks>
        /// This action can only be performed by a member of a private chat or a server member with an appropriate role
        /// </remarks>
        /// <param name="messageId">string ObjectId representation of the message to unpin</param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested message is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPatch("{messageId}/unpin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> UnpinMessage(string messageId)
        {
            UnpinMessageCommand command = new() { MessageId = messageId };
            await Mediator.Send(command);

            await Mediator.Send(new NotifyMessageUpdatedQuery { MessageId = messageId });

            return NoContent();
        }
    }
}