using Application.Models;
using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.Users.SendMessageToUser
{
    public record SendMessageToUserRequest : IRequest<MessageChatDto>
    {
        /// <summary>
        /// The unique identifier of the user to whom the message will be sent.
        /// </summary>
        [DefaultValue(1)]
        public int UserId { get; init; }

        /// <summary>
        /// The text of the message to be sent.
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
