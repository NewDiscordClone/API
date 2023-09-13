using Microsoft.AspNetCore.SignalR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.WebApi.Hubs;

namespace Sparkle.WebApi.Providers
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