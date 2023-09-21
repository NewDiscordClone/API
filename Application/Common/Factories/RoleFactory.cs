using Microsoft.AspNetCore.Identity;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Factories
{
    public class RoleFactory : IRoleFactory
    {
        private readonly IAppDbContext _context;

        public RoleFactory(IAppDbContext context)
        {
            _context = context;
        }

        public List<Role> GetDefaultServerRoles(string serverId)
        {
            Role ownerRole = new()
            {
                Name = "Owner",
                Color = "#FFF000",
                IsAdmin = true,
                ServerId = serverId
            };


            Role memberRole = new()
            {
                Name = "Member",
                Color = "#FFF000",
                ServerId = serverId
            };

            _context.SqlRoles.AddMany(new[] { ownerRole, memberRole });
            _context.AddClaimToRoleAsync(memberRole, new IdentityRoleClaim<Guid>()
            {
                ClaimType = ServerClaims.ChangeServerName,
                ClaimValue = true.ToString(),
                RoleId = memberRole.Id
            });

            return new() { ownerRole, memberRole };
        }
    }
}
