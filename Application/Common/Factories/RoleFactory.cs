using Microsoft.AspNetCore.Identity;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Factories
{
    public class RoleFactory : IRoleFactory
    {
        // это не финальные наборы клеймом, а наборы клеймом для тестов

        //TODO: Add more claims
        private readonly string[] _serverMemberDefaultClaims
            = { ServerClaims.ChangeServerName };

        //TODO: Add more claims
        private readonly string[] _serverOwnerDefaultClaims =
        {
            ServerClaims.ChangeServerName,
            ServerClaims.ManageServer,
            ServerClaims.ManageRoles,
            ServerClaims.ManageChannels
        };
        //TODO: Add more claims

        private readonly string[] _personalChatMemberClaims =
        {
            ServerClaims.ManageRoles,
            ServerClaims.ManageServer,
            ServerClaims.ManageMessages,
            ServerClaims.ManageChannels
        };

        private readonly string[] _groupChatMemberClaims =
        {
            ServerClaims.ManageRoles,
            ServerClaims.ManageServer,
            ServerClaims.ManageMessages,
            ServerClaims.ManageChannels
        };

        private readonly string[] _groupChatOwnerClaims =
        {
            ServerClaims.ManageMessages,
        };

        private static List<ServerClaim> CreateClaimsForRole(Role role, params string[] claims)
        {
            if (!claims.All(claim => ServerClaims.GetClaims().Contains(claim)))
                throw new ArgumentException("Invalid claim(s) provided");

            List<ServerClaim> serverClaims = new();

            foreach (string claim in claims)
            {
                ServerClaim serverClaim = new()
                {
                    ClaimType = claim,
                    RoleId = role.Id
                };
                serverClaims.Add(serverClaim);
            }

            return serverClaims;
        }

        public List<Role> GetDefaultServerRoles(string serverId)
        {
            Role ownerRole = new()
            {
                Name = Constants.Constants.ServerProfile.DefaultOwnerRoleName,
                Color = "#FFF000",
                IsAdmin = true,
                ServerId = serverId,
                Priority = 100
            };


            Role memberRole = new()
            {
                Name = Constants.Constants.ServerProfile.DefaultMemberRoleName,
                Color = "#FFF000",
                ServerId = serverId,
                Priority = 0
            };

            memberRole.Claims = CreateClaimsForRole(memberRole, _serverMemberDefaultClaims)
                .ConvertAll(c => c as IdentityRoleClaim<Guid>);

            ownerRole.Claims = CreateClaimsForRole(memberRole, _serverOwnerDefaultClaims)
                .ConvertAll(c => c as IdentityRoleClaim<Guid>);

            return new() { ownerRole, memberRole };
        }

        public Role GetRoleForPersonalChat(string chatId)
        {
            Role role = new()
            {
                Name = Constants.Constants.ServerProfile.DefaultMemberRoleName,
                Color = "#FFF000",
                ChatId = chatId,
                Priority = 0
            };

            role.Claims = CreateClaimsForRole(role, _personalChatMemberClaims)
                .ConvertAll(c => c as IdentityRoleClaim<Guid>);

            return role;
        }

        public List<Role> GetGroupChatRoles(string chatId)
        {
            Role ownerRole = new()
            {
                Name = Constants.Constants.ServerProfile.DefaultOwnerRoleName,
                Color = "#FFF000",
                IsAdmin = true,
                ChatId = chatId,
                Priority = 1
            };


            Role memberRole = new()
            {
                Name = Constants.Constants.ServerProfile.DefaultMemberRoleName,
                Color = "#FFF000",
                ChatId = chatId,
                Priority = 0
            };

            memberRole.Claims = CreateClaimsForRole(memberRole, _groupChatMemberClaims)
                .ConvertAll(c => c as IdentityRoleClaim<Guid>);

            ownerRole.Claims = CreateClaimsForRole(memberRole, _groupChatOwnerClaims)
                .ConvertAll(c => c as IdentityRoleClaim<Guid>);

            return new() { ownerRole, memberRole };
        }
    }
}
