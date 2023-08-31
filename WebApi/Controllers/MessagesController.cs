using Application.Commands.HubClients.Messages.MessageAdded;
using Application.Commands.HubClients.Messages.MessageRemoved;
using Application.Commands.HubClients.Messages.MessageUpdated;
using Application.Commands.Messages.AddMessage;
using Application.Commands.Messages.AddReaction;
using Application.Commands.Messages.EditMessage;
using Application.Commands.Messages.PinMessage;
using Application.Commands.Messages.RemoveAllReactions;
using Application.Commands.Messages.RemoveAttachment;
using Application.Commands.Messages.RemoveMessage;
using Application.Commands.Messages.RemoveReaction;
using Application.Commands.Messages.UnpinMessage;
using Application.Common.Exceptions;
using Application.Models;
using Application.Providers;
using Application.Queries.GetMessages;
using Application.Queries.GetPinnedMessages;
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
    public class MessagesController : ApiControllerBase
    {
        public MessagesController(IMediator mediator, IAuthorizedUserProvider userProvider) : base(mediator,
            userProvider)
        {
        }

        /// <summary>
        /// Loads a page of Messages from the given chat to show them in client app. The size of a page defined as a constant (see <see cref="GetMessagesRequestHandler._pageSize"/>)
        /// </summary>
        /// <param name="chatId">string ObjectId representation of the chat to get pinned messages from</param>
        /// <param name="messagesCount">The amount of messages that already loaded to skip them. Set 0 to load last messages
        /// </param>
        /// <returns>A list of messages to show</returns>
        /// <response code="200">Ok. A list of messages to show</response>
        /// <response code="400">Bad Request. The requested chat is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client must be a member of the chat</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<List<Message>>> GetMessages(string chatId, int messagesCount)
        {
            try
            {
                List<Message> messages = await Mediator.Send(new GetMessagesRequest()
                    { ChatId = chatId, MessagesCount = messagesCount });
                return Ok(messages);
            }
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Loads all of the Messages that are pinned in the given chat
        /// </summary>
        /// <param name="chatId">
        /// string ObjectId representation of the chat to get pinned messages from
        /// </param>
        /// <returns>A list of pinned messages in the chat</returns>
        /// <response code="200">Ok. A list of pinned messages in the chat</response>
        /// <response code="400">Bad Request. The requested chat is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client must be a member of the chat</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<List<Message>>> GetPinnedMessages(string chatId)
        {
            try
            {
                List<Message> messages = await Mediator.Send(new GetPinnedMessagesRequest() { ChatId = chatId });
                return Ok(messages);
            }
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Adds message to the given chat and notify other members about it
        /// </summary>
        /// <param name="request">
        /// ```
        /// text: string // Up to 2000 characters
        /// chatId: string // represents ObjectId of the chat to send the message to
        /// attachments: Attachment[] // Attachments that user includes to the message
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested chat is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> AddMessage([FromBody] AddMessageRequest request)
        {
            try
            {
                Message message = await Mediator.Send(request);
                await Mediator.Send(new NotifyMessageAddedRequest { MessageId = message.Id});
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
        /// Adds reaction to the message
        /// </summary>
        /// <param name="request"> 
        /// ```
        /// messageId: string // represents ObjectId of the message to add reaction to
        /// emoji: string // represents emoji name in colon brackets (:smile:)
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested message is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client must be a member of the chat</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> AddReaction([FromBody] AddReactionRequest request)
        {
            try
            {
                await Mediator.Send(request);
                await Mediator.Send(new NotifyMessageUpdatedRequest { MessageId = request.MessageId });
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
        /// A request to change the given message text
        /// </summary>
        /// <remarks>
        /// This action can only be performed by the owner of the message
        /// </remarks>
        /// <param name="request">
        /// ```
        /// messageId: string // represents ObjectId of the message to edit
        /// newText: string // provided text to change the previous one
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested message is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client must be the owner of the message</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> EditMessage([FromBody] EditMessageRequest request)
        {
            try
            {
                await Mediator.Send(request);
                await Mediator.Send(new NotifyMessageUpdatedRequest { MessageId = request.MessageId });
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
        /// Pins the selected message to the given chat
        /// </summary>
        /// <remarks>
        /// This action can only be performed by a member of a private chat or a server member with an appropriate role
        /// </remarks>
        /// <param name="request">
        /// ```
        /// messageId: string // represents ObjectId of the message to pin
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested message is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> PinMessage([FromBody] PinMessageRequest request)
        {
            try
            {
                await Mediator.Send(request);
                //TODO: Реалізація відправки Message
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
        /// Remove all reactions from the given message.
        /// </summary>
        /// <remarks>
        /// This action can only be performed by a server member with an appropriate role
        /// </remarks>
        /// <param name="request">
        /// ```
        /// messageId: string // represents ObjectId of the message to remove reactions from
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested message is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RemoveAllReactions([FromBody] RemoveAllReactionsRequest request)
        {
            try
            {
                await Mediator.Send(request);
                await Mediator.Send(new NotifyMessageUpdatedRequest { MessageId = request.MessageId });
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
        /// Remove a selected attachment from the given message
        /// </summary>
        /// <remarks>
        /// This actions can only be performed by the owner of the message
        /// </remarks>
        /// <param name="request">
        /// ```
        /// messageId: string // represents ObjectId of the message to remove the attachment from
        /// attachmentIndex: int // the index of the attachment to remove
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested message or attachment is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client must to be the owner of the message</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RemoveAttachment([FromBody] RemoveAttachmentRequest request)
        {
            try
            {
                await Mediator.Send(request);
                await Mediator.Send(new NotifyMessageUpdatedRequest { MessageId = request.MessageId });
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
        /// Removes the given message with it's attachments and reactions
        /// </summary>
        /// <remarks>
        /// This action can only be performed by the owner of the message or a server member with an appropriate role
        /// </remarks>
        /// <param name="request">
        /// ```
        /// messageId: string // represents ObjectId of the message to remove
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested message is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RemoveMessage([FromBody] RemoveMessageRequest request)
        {
            try
            {
                Chat chat = await Mediator.Send(request);
                await Mediator.Send(new NotifyMessageRemovedRequest { MessageId = request.MessageId, ChatId = chat.Id});
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
        /// Remove the given reaction you have added
        /// </summary>
        /// <remarks>
        /// this action can only be performed by the owner of the reaction
        /// </remarks>
        /// <param name="request">
        /// ```
        /// messageId: string // represents ObjectId of the message to remove
        /// reactionIndex: string // the index of the reaction to remove
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested message or reaction is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client must to be the owner of the reaction</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RemoveReaction([FromBody] RemoveReactionRequest request)
        {
            try
            {
                await Mediator.Send(request);
                await Mediator.Send(new NotifyMessageUpdatedRequest { MessageId = request.MessageId });
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
        /// Unpin previously pinned message
        /// </summary>
        /// <remarks>
        /// This action can only be performed by a member of a private chat or a server member with an appropriate role
        /// </remarks>
        /// <param name="request">
        /// ```
        /// messageId: string // represents ObjectId of the message to remove
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested message is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> UnpinMessage([FromBody] UnpinMessageRequest request)
        {
            try
            {
                Message chat = await Mediator.Send(request);
                //TODO: Реалізація відправки Message
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
    }
}