using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Users.Queries.GetUserDetails
{
    public class GetUserDetailsQueryValidator : AbstractValidator<GetUserDetailsQuery>
    {
        public GetUserDetailsQueryValidator()
        {
            RuleFor(q => q.ServerId)!.IsObjectId();
        }
    }
}
