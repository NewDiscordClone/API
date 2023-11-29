using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Repositories
{
    public class UserRepository(PostgresDbContext context)
        : Repository<PostgresDbContext, User, Guid>(context), IUserRepository
    {
    }
}