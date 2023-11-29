using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Repositories
{
    public class ServerRepository(MongoDbContext context)
        : Repository<MongoDbContext, Server, string>(context), IServerRepository
    {
    }
}