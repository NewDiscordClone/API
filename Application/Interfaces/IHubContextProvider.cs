using Microsoft.AspNetCore.SignalR;

namespace Application.Interfaces
{
    public interface IHubContextProvider
    {
        IHubClients Clients { get; }
    }
}