using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Invitations.Queries.GetInvitationDetails
{
    public record GetInvitationDetailsRequest : IRequest<InvitationDetailsDto>
    {
        /// <summary>
        /// The unique identifier of the invitation for which to retrieve details
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string InvitationId { get; init; }
    }
}
