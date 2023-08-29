using MediatR;

namespace Application.Interfaces
{
    public interface IServerRequest : IRequest
    {
        int ServerId { get; }
    }
    public interface IServerRequest<TResponse> : IRequest<TResponse>
    {
        int ServerId { get; }
    }
}
