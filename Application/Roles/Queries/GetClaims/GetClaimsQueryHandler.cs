using MediatR;
using Sparkle.Application.Common;

namespace Sparkle.Application.Roles.Queries.GetClaims
{
    public class GetClaimsQueryHandler : IRequestHandler<GetClaimsQuery, IEnumerable<string>>
    {
        public async Task<IEnumerable<string>> Handle(GetClaimsQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return ServerClaims.GetClaims();
        }
    }
}
