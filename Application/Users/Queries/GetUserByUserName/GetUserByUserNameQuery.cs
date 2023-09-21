using System.ComponentModel;
using MediatR;
using Sparkle.Application.Users.Queries.GetUserDetails;

namespace Sparkle.Application.Users.Queries.GetUserDetails
{
    public record GetUserByUserNameQuery : IRequest<GetUserDetailsDto>
    {
        /// <summary>
        /// The unique username of the user to retrieve details for
        /// </summary>
        public string UserName { get; init; }
        /// <summary>
        /// Id of the server for providing user's ServerProfile for this server
        /// </summary>
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string? ServerId { get; init; }
    }
}