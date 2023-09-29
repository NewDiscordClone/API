using Sparkle.Contracts.Roles;

namespace Sparkle.Contracts.Servers
{
    public record ServerProfileLookupResponse
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string UserId { get; init; }
        public RoleResponse MainRole { get; init; }
    }

    public record ServerProfileResponse
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string UserId { get; init; }
        public List<RoleResponse> Roles { get; init; }
    }
}
