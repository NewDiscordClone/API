using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Common.Factories
{
    public class RoleFactory : IRoleFactory
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleFactory(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public List<Role> GetDefaultServerRoles()
        {
            Role ownerRole = new()
            {
                Name = "Owner",
                Color = "#FFF000"
            };

            _roleManager.AddClaimAsync(ownerRole, new Claim("Admin", "true"));

            Role memberRole = new()
            {
                Name = "Member",
                Color = "#FFF000"
            };

            _roleManager.AddClaimAsync(ownerRole, new Claim("SendMessages", "true"));

            return new() { ownerRole, memberRole };
        }
    }
}
