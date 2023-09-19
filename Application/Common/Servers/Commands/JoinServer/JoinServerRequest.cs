using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Sparkle.Application.Common.Servers.Queries.GetServerDetails;

namespace Sparkle.Application.Common.Servers.Commands.JoinServer
{
    public record JoinServerRequest : IRequest<ServerDetailsDto>
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string InvitationId { get; set; }
    }
}