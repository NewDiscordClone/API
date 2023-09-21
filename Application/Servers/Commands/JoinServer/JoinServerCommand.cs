using MediatR;
using System.ComponentModel;
using Sparkle.Application.Servers.Queries.ServerDetails;

namespace Sparkle.Application.Servers.Commands.JoinServer
{
    public record JoinServerCommand : IRequest<ServerDetailsDto>
    {
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string InvitationId { get; set; }
    }
}