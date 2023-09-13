using Application.Models;
using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Users.Commands.SendMessageToUser
{
    public record SendMessageToUserRequest : IRequest<MessageChatDto>
    {
        /// <summary>
        /// The unique identifier of the user to send the message to.
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        /// The text of the message, May contain links
        /// </summary>
        [MaxLength(2000)]
        [DefaultValue("MessageText")]
        public string Text { get; init; }

        /// <summary>
        /// Optional attachments to include with the message.
        /// </summary>
        public List<Attachment>? Attachments { get; init; }
    }
}