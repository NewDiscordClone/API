using AutoMapper;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Relationships.Queries.GetRelationships
{

    public record UserLookupViewModel : IMapWith<User>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Gets or sets the unique username of the user.
        /// </summary>
        public string Username { get; init; }

        /// <summary>
        /// Gets or sets the display name of the user.
        /// </summary>
        public string? DisplayName { get; init; }

        /// <summary>
        /// Gets or sets the avatar URL of the user.
        /// </summary>
        public string? AvatarUrl { get; init; }

        /// <summary>
        /// Gets or sets the status of the user.
        /// </summary>
        public UserStatus Status { get; init; }

        /// <summary>
        /// Configures the mapping between the User and UserLookupViewModel types.
        /// </summary>
        /// <param name="profile">The AutoMapper profile.</param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserLookupViewModel>();
        }
    }
}