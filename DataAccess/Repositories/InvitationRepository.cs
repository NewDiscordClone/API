using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Repositories
{
    public class InvitationRepository(MongoDbContext context)
        : Repository<MongoDbContext, Invitation, string>(context), IInvitationRepository
    {
    }
}