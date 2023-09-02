using MediatR;

namespace Application.Commands.Servers.BanUser
{
    public class BanUserRequest : IRequest
    {
        public string ServerId { get; set; }
        public int UserId { get; set; }
    }
}