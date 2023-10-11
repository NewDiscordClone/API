using Microsoft.AspNetCore.Authorization;

namespace Sparkle.WebApi.Authorization.Requirements
{
    /// <summary>
    /// Represents an authorization requirement that is satisfied if either of two specified requirements is satisfied.
    /// </summary>
    public class OrRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// The first requirement that can satisfy the authorization.
        /// </summary>
        public IAuthorizationRequirement Firstly { get; init; }

        /// <summary>
        /// The second requirement that can satisfy the authorization.
        /// </summary>
        public IAuthorizationRequirement Secondary { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrRequirement"/> class with the specified requirements.
        /// </summary>
        /// <param name="firstly">The first requirement that can satisfy the authorization.</param>
        /// <param name="secondary">The second requirement that can satisfy the authorization.</param>
        public OrRequirement(IAuthorizationRequirement firstly, IAuthorizationRequirement secondary)
        {
            Firstly = firstly;
            Secondary = secondary;
        }
    }

    /// <summary>
    /// Extension methods for adding an OR requirement to an authorization policy.
    /// </summary>
    public static class OrRequirementExtensions
    {
        /// <summary>
        /// Adds an OR requirement to the authorization policy builder.
        /// </summary>
        /// <param name="builder">The authorization policy builder.</param>
        /// <param name="firstly">The first authorization requirement.</param>
        /// <param name="secondary">The second authorization requirement.</param>
        /// <returns>The authorization policy builder with the OR requirement added.</returns>
        public static AuthorizationPolicyBuilder AddOrRequirement(this AuthorizationPolicyBuilder builder, IAuthorizationRequirement firstly, IAuthorizationRequirement secondary)
        {
            return builder.AddRequirements(new OrRequirement(firstly, secondary));
        }

        /// <summary>
        /// Creates a new OR requirement.
        /// </summary>
        /// <param name="firstly">The first authorization requirement.</param>
        /// <param name="secondary">The second authorization requirement.</param>
        /// <returns>The new OR requirement.</returns>
        public static OrRequirement Or(this IAuthorizationRequirement firstly, IAuthorizationRequirement secondary)
        {
            return new OrRequirement(firstly, secondary);
        }
    }
}
