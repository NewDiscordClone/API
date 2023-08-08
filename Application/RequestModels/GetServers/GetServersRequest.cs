using MediatR;

namespace Application.RequestModels.GetServer
{
    public class GetServersRequest : IRequest<List<GetServerLookupDto>>
    {
        public int UserId { get; init; }
    }
}