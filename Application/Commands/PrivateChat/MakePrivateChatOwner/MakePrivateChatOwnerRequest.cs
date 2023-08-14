using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Commands.PrivateChat.MakePrivateChatOwner
{
    public class MakePrivateChatOwnerRequest : IRequest
    {
        public int ChatId { get; init; }
        public int MemberId { get; init; }
    }
}