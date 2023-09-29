using MediatR;

namespace Sparkle.Application.Servers.Roles.Queries.GetClaims
{
    public class GetClaimsQuery : IRequest<IEnumerable<string>>
    {
    }
}
