using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces
{
    public interface IUserProfileProvider
    {
        List<UserProfile> Profiles { get; }
    }
}
