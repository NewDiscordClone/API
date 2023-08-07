using MediatR;

namespace Application.RequestModels.GetServer
{
    public class GetServersRequest : IRequest<List<GetServerDto>>
    {
        public int UserId { get; init; }
    }
}