using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sparkle.Application.Common.Constants;
using Sparkle.Domain;

namespace Sparkle.DataAccess.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            Role groupChatOwnerRole = new()
            {
                Id = Constants.Roles.GroupChatOwnerId,
                Name = Constants.Roles.GroupChatOwnerName,
                Color = Constants.Roles.DefaultColor,
                ServerId = null,
                Priority = 1,
                IsAdmin = true,
            };

            Role groupChatMemberRole = new()
            {
                Id = Constants.Roles.GroupChatMemberId,
                Name = Constants.Roles.GroupChatMemberName,
                Color = Constants.Roles.DefaultColor,
                ServerId = null,
                Priority = 0,
            };

            Role personalChatMemberRole = new()
            {
                Id = Constants.Roles.PrivateChatMemberId,
                Name = Constants.Roles.PrivateChatMemberName,
                Color = Constants.Roles.DefaultColor,
                ServerId = null,
                Priority = 0,
            };

            Role serverOwnerRole = new()
            {
                Id = Constants.Roles.ServerOwnerId,
                Name = Constants.Roles.ServerOwnerName,
                Color = Constants.Roles.DefaultColor,
                ServerId = null,
                Priority = 100,
                IsAdmin = true,
            };

            Role serverMemberRole = new()
            {
                Id = Constants.Roles.ServerMemberId,
                Name = Constants.Roles.ServerMemberName,
                Color = Constants.Roles.DefaultColor,
                ServerId = null,
                Priority = 0,
            };

            builder.HasData(groupChatOwnerRole,
                            groupChatMemberRole,
                            personalChatMemberRole,
                            serverOwnerRole,
                            serverMemberRole);
        }
    }
}
