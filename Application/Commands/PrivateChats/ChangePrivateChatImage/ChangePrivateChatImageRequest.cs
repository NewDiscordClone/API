using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Commands.PrivateChats.ChangePrivateChatImage
{
    public class ChangePrivateChatImageRequest : IRequest
    {
        public int ChatId { get; init; }
        public string NewImage { get; init; }
    }
}