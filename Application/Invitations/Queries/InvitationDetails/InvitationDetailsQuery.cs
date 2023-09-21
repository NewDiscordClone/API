using MediatR;
using System.ComponentModel;

namespace Sparkle.Application.Invitations.Queries.InvitationDetails
{
    public record InvitationDetailsQuery : IRequest<InvitationDetailsDto>
    {
        /// <summary>
        /// The unique identifier of the invitation for which to retrieve details
        /// </summary>
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string InvitationId { get; init; }
    }
}
