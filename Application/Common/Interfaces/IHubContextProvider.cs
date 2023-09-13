using Microsoft.AspNetCore.SignalR;

namespace Application.Common.Interfaces
{
    public interface IHubContextProvider
    {
        IHubClients Clients { get; }
    }
}