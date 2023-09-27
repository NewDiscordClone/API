using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sparkle.Application.Common;
using Sparkle.Application.Common.Constants;

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
                    RoleId = Constants.Roles.PrivateChatMemberId,
                    ClaimType = ServerClaims.ManageRoles,
                    ClaimValue = "true"
                },
                new IdentityRoleClaim<Guid>
                {
                    Id = id++,
                    RoleId = Constants.Roles.PrivateChatMemberId,
                    ClaimType = ServerClaims.ManageServer,
                    ClaimValue = "true"
                },
                new IdentityRoleClaim<Guid>
                {
                    Id = id++,
                    RoleId = Constants.Roles.PrivateChatMemberId,
                    ClaimType = ServerClaims.ManageMessages,
                    ClaimValue = "true"
                },
                new IdentityRoleClaim<Guid>
                {
                    Id = id++,
                    RoleId = Constants.Roles.PrivateChatMemberId,
                    ClaimType = ServerClaims.ManageChannels,
                    ClaimValue = "true"
                },
        
                // Group Chat Member Claims
                new IdentityRoleClaim<Guid>
                {
                    Id = id++,
                    RoleId = Constants.Roles.GroupChatMemberId,
                    ClaimType = ServerClaims.ManageRoles,
                    ClaimValue = "true"
                },
                new IdentityRoleClaim<Guid>
                {
                    Id = id++,
                    RoleId = Constants.Roles.GroupChatMemberId,
                    ClaimType = ServerClaims.ManageServer,
                    ClaimValue = "true"
                },
                new IdentityRoleClaim<Guid>
                {
                    Id = id++,
                    RoleId = Constants.Roles.GroupChatMemberId,
                    ClaimType = ServerClaims.ManageMessages,
                    ClaimValue = "true"
                },
                new IdentityRoleClaim<Guid>
                {
                    Id = id++,
                    RoleId = Constants.Roles.GroupChatMemberId,
                    ClaimType = ServerClaims.ManageChannels,
                    ClaimValue = "true"
                },

                // Group Chat Owner Claims
                new IdentityRoleClaim<Guid>
                {
                    Id = id++,
                    RoleId = Constants.Roles.GroupChatOwnerId,
                    ClaimType = ServerClaims.ManageMessages,
                    ClaimValue = "true"
                }
            };

            builder.HasData(data);
        }

    }
}
