using Application.Interfaces;
using Application.Models;
using Application.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WebApi.Authorization
{
    public record ServerClaimsAuthorizationRequirement : IAuthorizationRequirement
    {
        public int ServerId { get; init; }
        public IEnumerable<string> ClaimTypes { get; init; }

        public ServerClaimsAuthorizationRequirement(int serverId, IEnumerable<string> claimTypes)
        {
            ServerId = serverId;
            ClaimTypes = claimTypes;
        }
    }

    public class ServerClaimsAuthorizationRequirementHandler :
        AuthorizationHandler<ServerClaimsAuthorizationRequirement>
    {
        private readonly IAppDbContext _context;
        private readonly IAuthorizedUserProvider _userProvider;
        private readonly RoleManager<Role> _roleManager;

        public ServerClaimsAuthorizationRequirementHandler(IAppDbContext context,
            IAuthorizedUserProvider userProvider,
            RoleManager<Role> roleManager)
        {
            _context = context;
            _userProvider = userProvider;
            _roleManager = roleManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ServerClaimsAuthorizationRequirement requirement)
        {
            ServerProfile? profile = await _context.ServerProfiles.FirstAsync(profile
                => profile.Server.Id == requirement.ServerId
                && profile.User.Id == _userProvider.GetUserId());

            if (profile is null || profile.Roles is null)
                return;

            List<string> matchingUserClaims = new();
            foreach (Role role in profile.Roles)
            {
                foreach (Claim claim in await _roleManager.GetClaimsAsync(role))
                {
                    if (requirement.ClaimTypes.Any(claimType
                        => string.Equals(claim.Type, claimType)))
                    {
                        matchingUserClaims.Add(claim.Type);
                    }

                    if (requirement.ClaimTypes.All(matchingUserClaims.Contains))
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }
            }
        }
    }
}