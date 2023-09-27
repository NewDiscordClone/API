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

        private readonly Role _groupChatOwnerRole = new()
        {
            Id = Constants.Constants.Roles.GroupChatOwnerId,
            Name = Constants.Constants.Roles.GroupChatOwnerName,
            Color = Constants.Constants.Roles.DefaultColor,
            ChatId = null,
            ServerId = null,
            Priority = 1,
        };

        private readonly Role _groupChatMemberRole = new()
        {
            Id = Constants.Constants.Roles.GroupChatMemberId,
            Name = Constants.Constants.Roles.GroupChatMemberName,
            Color = Constants.Constants.Roles.DefaultColor,
            ChatId = null,
            ServerId = null,
            Priority = 0,
        };

        private readonly Role _personalChatMemberRole = new()
        {
            Id = Constants.Constants.Roles.PrivateChatMemberId,
            Name = Constants.Constants.Roles.PrivateChatMemberName,
            Color = Constants.Constants.Roles.DefaultColor,
            ChatId = null,
            ServerId = null,
            Priority = 0,
        };

        public Role PersonalChatMemberRole => _personalChatMemberRole;

        public Role GroupChatMemberRole => _groupChatMemberRole;

        public Role GroupChatOwnerRole => _groupChatOwnerRole;

        public string[] GroupChatOwnerClaims => _groupChatOwnerClaims;

        public string[] GroupChatMemberClaims => _groupChatMemberClaims;

        public string[] PersonalChatMemberClaims => _personalChatMemberClaims;

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

        public List<Role> GetGroupChatRoles()
        {
            Role ownerRole = GroupChatOwnerRole;
            Role memberRole = GroupChatMemberRole;

            return new() { ownerRole, memberRole };
        }
    }
}
