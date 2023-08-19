using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Commands.PrivateChats.AddMemberToPrivateChat
{
    public class AddMemberToPrivateChatRequest : IRequest
    {
        public int ChatId { get; init; }
        public int NewMemberId { get; init; }
    }
}