using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sparkle.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparkle.DataAccess.Configurations
{
    internal class UserConnectionsConfiguration : IEntityTypeConfiguration<UserConnections>
    {
        public void Configure(EntityTypeBuilder<UserConnections> builder)
        {
            builder.HasKey(c => c.UserId);
        }
    }
}
