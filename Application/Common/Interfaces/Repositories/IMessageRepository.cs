using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces.Repositories
{
    public interface IMessageRepository : IRepository<Message, string>
    {
    }
}