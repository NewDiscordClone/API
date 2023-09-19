using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Servers.Queries.ServerDetails
{
    public class ServerDetailsQueryValidator : AbstractValidator<ServerDetailsQuery>
    {
        public ServerDetailsQueryValidator()
        {
            RuleFor(x => x.ServerId).NotNull().IsObjectId();
        }
    }
}
