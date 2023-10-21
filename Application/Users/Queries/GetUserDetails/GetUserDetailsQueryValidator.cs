using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Users.Queries
{
    public class GetUserDetailsQueryValidator : AbstractValidator<GetUserDetailsQuery>
    {
        public GetUserDetailsQueryValidator()
        {
            RuleFor(q => q.ServerId)!.IsObjectId();
        }
    }
}
