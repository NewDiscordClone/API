using Application.Models;
using MediatR;

namespace Application.Commands.Invitations.MakeInvitation
{
    public record MakeInvitationRequest : IRequest<string>
    {
        public string ServerId { get; set; }
        public bool IncludeUser { get; set; }
        public DateTime? ExpireTime { get; set; }
    }
}