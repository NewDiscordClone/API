using MediatR;

namespace Application.Commands.Invitations.CreateInvitation
{
    public record CreateInvitationRequest : IRequest<string>
    {
        public string ServerId { get; set; }
        public bool IncludeUser { get; set; }
        public DateTime? ExpireTime { get; set; }
    }
}