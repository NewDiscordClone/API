using MediatR;

namespace Application.Commands.Servers.UnbanUser
{
    public class UnbanUserRequest : IRequest
    {
        public string ServerId { get; set; }
        public int UserId { get; set; }
    }
}