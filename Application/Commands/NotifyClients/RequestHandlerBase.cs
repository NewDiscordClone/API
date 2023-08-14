using Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Application.Commands.NotifyClients
{
    public class RequestHandlerBase
    {
        private readonly IHubContextProvider _hubContextProvider;
        protected IHubClients Clients => _hubContextProvider.Clients;
        

        public RequestHandlerBase(IHubContextProvider hubContextProvider)
        {
            _hubContextProvider = hubContextProvider;
        }
    }
}