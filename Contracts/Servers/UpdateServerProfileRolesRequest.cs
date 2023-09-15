namespace Sparkle.Contracts.Servers
{
    /// <summary>
    /// Request for updating server profile roles
    /// </summary>
    public record UpdateServerProfileRolesRequest
    {
        /// <summary>
        /// List of roles to be assigned to the user
        /// </summary>
        public List<Guid> Roles { get; init; }
    }
}
