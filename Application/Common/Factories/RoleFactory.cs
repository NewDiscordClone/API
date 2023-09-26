using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Factories
{
    public class RoleFactory : IRoleFactory
    {
        // это не финальные наборы клеймом, а наборы клеймом для тестов

        private readonly IRoleRepository _roleRepository;

        public RoleFactory(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

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

        private async Task<List<ServerClaim>> CreateClaimsForRoleAsync(Role role, params string[] claims)
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

            await _roleRepository.AddClaimsToRoleAsync(role, serverClaims);

            return serverClaims;
        }

        public async Task<List<Role>> GetDefaultServerRolesAsync(string serverId)
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

            await _roleRepository.AddAsync(ownerRole);
            await _roleRepository.AddAsync(memberRole);

            await CreateClaimsForRoleAsync(memberRole, _serverMemberDefaultClaims);
            await CreateClaimsForRoleAsync(ownerRole, _serverOwnerDefaultClaims);

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

            //   CreateClaimsForRole(role, _personalChatMemberClaims);

            //  _roleRepository.AddClaimsToRoleAsync(role, roleClaims);

            return role;
        }

        public List<Role> GetGroupChatRoles(string chatId)
        {
            Role ownerRole = GetGroupChatOwnerRole(chatId);
            Role memberRole = GetGroupChatMemberRole(chatId);

            return new() { ownerRole, memberRole };
        }

        public Role GetGroupChatMemberRole(string chatId)
        {
            Role memberRole = new()
            {
                Name = Constants.Constants.ServerProfile.DefaultMemberRoleName,
                Color = "#FFF000",
                ChatId = chatId,
                Priority = 0
            };

            //     List<IdentityRoleClaim<Guid>> memberRoleClaims = CreateClaimsForRole(memberRole, _groupChatMemberClaims)
            //        .ConvertAll(c => c as IdentityRoleClaim<Guid>);

            //     _roleRepository.AddClaimsToRoleAsync(memberRole, memberRoleClaims);

            return memberRole;
        }

        public Role GetGroupChatOwnerRole(string chatId)
        {
            Role ownerRole = new()
            {
                Name = Constants.Constants.ServerProfile.DefaultOwnerRoleName,
                Color = "#FFF000",
                IsAdmin = true,
                ChatId = chatId,
                Priority = 1
            };

            //     List<IdentityRoleClaim<Guid>> ownerRoleClaims = CreateClaimsForRole(ownerRole, _groupChatOwnerClaims)
            //         .ConvertAll(c => c as IdentityRoleClaim<Guid>);

            //    _roleRepository.AddClaimsToRoleAsync(ownerRole, ownerRoleClaims);

            return ownerRole;
        }
    }
}
