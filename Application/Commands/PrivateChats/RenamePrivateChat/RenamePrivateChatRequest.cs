using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Commands.PrivateChats.RenamePrivateChat
{
    public class RenamePrivateChatRequest : IRequest
    {
        public int ChatId { get; init; }
        public string NewTitle { get; init; }
    }
}