using MediatR;

namespace Application.Commands.Invitations.CreateInvitation
{
    public record CreateInvitationRequest : IRequest<string>
    {
        /// <summary>
        /// The unique identifier of the server for which to create an invitation.
        /// </summary>
        public string ServerId { get; set; }

        /// <summary>
        /// Indicates whether to include user information in the invitation.
        /// </summary>
        public bool IncludeUser { get; set; }

        /// <summary>
        /// The expiration time of the invitation. (Optional)
        /// </summary>
        public DateTime? ExpireTime { get; set; }
    }
}
