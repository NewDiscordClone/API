using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Invitations.Queries.InvitationDetails
{
    public class InvitationDetailsDto
    {
        /// <summary>
        /// The unique identifier of the invitation.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The user associated with the invitation. (Optional)
        /// </summary>
        public UserLookUp? User { get; set; }

        /// <summary>
        /// The server associated with the invitation.
        /// </summary>
        public ServerLookupDto Server { get; set; }

        /// <summary>
        /// The expiration time of the invitation. (Optional)
        /// </summary>
        public DateTime? ExpireTime { get; set; }
    }
}
