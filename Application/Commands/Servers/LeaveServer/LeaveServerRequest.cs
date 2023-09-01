using MediatR;

namespace Application.Commands.Servers.LeaveServer
{
    public record LeaveServerRequest : IRequest
    {
        public string ServerId { get; init; }
    }
}