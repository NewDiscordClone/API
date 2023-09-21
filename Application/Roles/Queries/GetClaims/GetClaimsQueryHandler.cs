using MediatR;
using Sparkle.Application.Common;
using System.Reflection;

namespace Sparkle.Application.Roles.Queries.GetClaims
{
    public class GetClaimsQueryHandler : IRequestHandler<GetClaimsQuery, IEnumerable<string>>
    {
        public async Task<IEnumerable<string>> Handle(GetClaimsQuery request, CancellationToken cancellationToken)
        {
            Type type = typeof(ServerClaims);

            List<string?> constants = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                .Select(x => x.GetRawConstantValue()?.ToString())
                .ToList();

            //filter null values
            List<string> result = new();
            foreach (string? constant in constants)
            {
                if (constant != null)
                {
                    result.Add(constant);
                }
            }

            await Task.CompletedTask;
            return result;
        }
    }
}
