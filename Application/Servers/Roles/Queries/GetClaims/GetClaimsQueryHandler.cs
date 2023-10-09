using MediatR;
using Sparkle.Application.Common.Constants;

namespace Sparkle.Application.Servers.Roles.Queries.GetClaims
{
    public class GetClaimsQueryHandler : IRequestHandler<GetClaimsQuery, IEnumerable<string>>
    {
        public async Task<IEnumerable<string>> Handle(GetClaimsQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return Constants.Claims.GetClaims();
        }
    }
}
