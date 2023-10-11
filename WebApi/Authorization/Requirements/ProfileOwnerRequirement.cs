using Microsoft.AspNetCore.Authorization;

namespace Sparkle.WebApi.Authorization.Requirements
{
    public class ProfileOwnerRequirement : IAuthorizationRequirement
    {
        public Guid ProfileId { get; init; }

        public ProfileOwnerRequirement(Guid profileId)
        {
            ProfileId = profileId;
        }
    }
}
