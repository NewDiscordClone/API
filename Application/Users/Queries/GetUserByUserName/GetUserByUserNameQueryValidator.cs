using FluentValidation;
using Sparkle.Application.Common.Constants;

namespace Sparkle.Application.Users.Queries.GetUserDetails
{
    public class GetUserByUserNameQueryValidator : AbstractValidator<GetUserByUserNameQuery>
    {
        public GetUserByUserNameQueryValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("Username is required.")
                .MaximumLength(Constants.User.UserNameMaxLength)
                .WithMessage($"Username must be {Constants.User.UserNameMaxLength} characters or less.")
                .Matches("^[a-z0-9_.]+$")
                .WithMessage("Username can only contain lowercase letters, numbers, '_', and '.'");
        }
    }
}