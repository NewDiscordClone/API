using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Commands.PrivateChat.RemovePrivateChatMember
{
    public class RemovePrivateChatMemberRequest : IRequest
    {
        public int ChatId { get; init; }
        public int MemberId { get; init; }
    }
}