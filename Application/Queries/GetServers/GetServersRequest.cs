using MediatR;

namespace Application.Queries.GetServer
{
    public class GetServersRequest : IRequest<List<GetServerLookupDto>>
    {
        public int UserId { get; init; }
    }
}