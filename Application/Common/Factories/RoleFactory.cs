using Application.Interfaces;
using Application.Models;
using System.Security.Claims;

namespace Application.Common.Factories
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

            _context.AddClaimToRoleAsync(memberRole, new Claim(ServerClaims.ChangeServerName, "true"));

            return new() { ownerRole, memberRole };
        }
    }
}
