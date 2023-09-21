using MediatR;

namespace Sparkle.Application.Roles.Queries.GetClaims
{
    public class GetClaimsQuery : IRequest<IEnumerable<string>>
    {
    }
}
