using Sparkle.Domain;
using Sparkle.Contracts.Roles;

namespace Sparkle.Contracts.Servers
{
    public record ServerProfileLookupResponse
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string UserId { get; init; }
        public string? AvatarUrl { get; set; }
        public string? TextStatus { get; set; }
        public UserStatus Status { get; set; }
        public RoleResponse MainRole { get; init; }
    }

    public record ServerProfileResponse
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string UserId { get; init; }
        public string? AvatarUrl { get; set; }
        public string? TextStatus { get; set; }
        public UserStatus Status { get; set; }
        public List<RoleResponse> Roles { get; init; }
    }
}
