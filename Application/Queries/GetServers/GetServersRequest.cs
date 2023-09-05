using MediatR;

namespace Application.Queries.GetServer
{
    public record GetServersRequest : IRequest<List<GetServerLookupDto>>
    {
    }
}