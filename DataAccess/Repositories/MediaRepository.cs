using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Repositories
{
    public class MediaRepository(MongoDbContext context)
        : Repository<MongoDbContext, Media, string>(context), IMediaRepository
    {

    }
}