using Application.Interfaces;
using MediatR;

namespace Application.Commands.NotifyClients.NotifyChatMembers
{
    public class NotifyChatMembersRequestHandler : RequestHandlerBase, IRequestHandler<NotifyChatMembersRequest>
    {
        
        public Task Handle(NotifyChatMembersRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        
        public NotifyChatMembersRequestHandler(IHubContextProvider hubContextProvider) : base(hubContextProvider)
        {
        }
    }
}