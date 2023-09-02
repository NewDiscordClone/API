using Application.Hubs;
using Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Providers
{
    public class HubContextProvider : IHubContextProvider
    {
        private readonly IHubContext<ChatHub> _context;
        public IHubClients Clients => _context.Clients;

        public HubContextProvider(IHubContext<ChatHub> context)
        {
            _context = context;
        }
    }
}