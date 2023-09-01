using MediatR;

namespace Application.Commands.Servers.JoinServer
{
    public record JoinServerRequest : IRequest
    {
        public string InvitationId { get; set; }
    }
}