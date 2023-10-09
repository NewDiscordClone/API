using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Sparkle.Application.Common.Constants.Constants;

namespace Sparkle.DataAccess.Configurations
{
    public class ClaimsConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder)
        {
            int id = 1;

            List<IdentityRoleClaim<Guid>> data = new()
            {
                // Personal Chat Member Claims
                new IdentityRoleClaim<Guid>
                {
                    Id = id++,
                    RoleId = Roles.PrivateChatMemberId,
                    ClaimType = Claims.ManageRoles,
                    ClaimValue = "true"
                },
                new IdentityRoleClaim<Guid>
                {
                    Id = id++,
                    RoleId = Roles.PrivateChatMemberId,
                    ClaimType = Claims.ManageServer,
                    ClaimValue = "true"
                },
                new IdentityRoleClaim<Guid>
                {
                    Id = id++,
                    RoleId = Roles.PrivateChatMemberId,
                    ClaimType = Claims.ManageMessages,
                    ClaimValue = "true"
                },
                new IdentityRoleClaim<Guid>
                {
                    Id = id++,
                    RoleId = Roles.PrivateChatMemberId,
                    ClaimType = Claims.ManageChannels,
                    ClaimValue = "true"
                },

                // Group Chat Member Claims
                new IdentityRoleClaim<Guid>
                {
                    Id = id++,
                    RoleId = Roles.GroupChatMemberId,
                    ClaimType = Claims.ManageRoles,
                    ClaimValue = "true"
                },
                new IdentityRoleClaim<Guid>
                {
                    Id = id++,
                    RoleId = Roles.GroupChatMemberId,
                    ClaimType = Claims.ManageServer,
                    ClaimValue = "true"
                },
                new IdentityRoleClaim<Guid>
                {
                    Id = id++,
                    RoleId = Roles.GroupChatMemberId,
                    ClaimType = Claims.ManageMessages,
                    ClaimValue = "true"
                },
                new IdentityRoleClaim<Guid>
                {
                    Id = id++,
                    RoleId = Roles.GroupChatMemberId,
                    ClaimType = Claims.ManageChannels,
                    ClaimValue = "true"
                },

                // Group Chat Owner Claims
                new IdentityRoleClaim<Guid>
                {
                    Id = id++,
                    RoleId = Roles.GroupChatOwnerId,
                    ClaimType = Claims.ManageMessages,
                    ClaimValue = "true"
                },

                // Server Member Claims
                new IdentityRoleClaim<Guid>
                {
                    Id = id++,
                    RoleId = Roles.ServerMemberId,
                    ClaimType = Claims.ChangeServerName,
                    ClaimValue = "true"
                },

                // Server Owner Claims
                new IdentityRoleClaim<Guid>
                {
                    Id = id++,
                    RoleId = Roles.ServerOwnerId,
                    ClaimType = Claims.ChangeServerName,
                    ClaimValue = "true"
                },
                new IdentityRoleClaim<Guid>
                {
                    Id = id++,
                    RoleId = Roles.ServerOwnerId,
                    ClaimType = Claims.ManageServer,
                    ClaimValue = "true"
                },
                new IdentityRoleClaim<Guid>
                {
                    Id = id++,
                    RoleId = Roles.ServerOwnerId,
                    ClaimType = Claims.ManageRoles,
                    ClaimValue = "true"
                },
                new IdentityRoleClaim<Guid>
                {
                    Id = id++,
                    RoleId = Roles.ServerOwnerId,
                    ClaimType = Claims.ManageChannels,
                    ClaimValue = "true"
                },
            };

            builder.HasData(data);
        }
    }
}
