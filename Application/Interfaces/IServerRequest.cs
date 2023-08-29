using MediatR;

namespace Application.Interfaces
{
    public interface IServerRequest : IRequest
    {
        string ServerId { get; }
    }
    public interface IServerRequest<TResponse> : IRequest<TResponse>
    {
        string ServerId { get; }
    }
}
