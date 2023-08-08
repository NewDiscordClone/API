using MediatR;

namespace Application.RequestModels.GetServer
{
    public class GetServersRequest : IRequest<List<GetServerLookUpDto>>
    {
        public int UserId { get; init; }
    }
}