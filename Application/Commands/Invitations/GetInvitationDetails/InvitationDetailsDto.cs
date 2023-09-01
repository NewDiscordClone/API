using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.Commands.Invitations.GetInvitationDetails
{
    public class InvitationDetailsDto
    {
        public string Id { get; set; }
        public UserLookUp? User { get; set; }
        public ServerLookupDto Server { get; set; }
        public DateTime? ExpireTime { get; set; }
    }
}