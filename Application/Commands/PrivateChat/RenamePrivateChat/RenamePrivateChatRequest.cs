using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Commands.PrivateChat.RenamePrivateChat
{
    public class RenamePrivateChatRequest : IRequest
    {
        public int ChatId { get; init; }
        public string NewTitle { get; init; }
    }
}