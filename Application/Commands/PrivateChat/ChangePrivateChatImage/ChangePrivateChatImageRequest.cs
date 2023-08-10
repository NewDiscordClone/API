using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Commands.PrivateChat.ChangePrivateChatImage
{
    public class ChangePrivateChatImageRequest : IRequest
    {
        public int ChatId { get; init; }
        public string NewImage { get; init; }
    }
}