using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Commands.Invitations.GetInvitationDetails
{
    public record GetInvitationDetailsRequest : IRequest<InvitationDetailsDto>
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string InvitationId { get; init; }
    }
}