using Microsoft.AspNetCore.SignalR;

namespace Sparkle.Application.Common.Interfaces
{
    public interface IHubContextProvider
    {
        IHubClients Clients { get; }
    }
}