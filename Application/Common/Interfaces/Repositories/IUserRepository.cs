using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User, Guid>
    {
    }
}
