using FluentValidation;
using Sparkle.Application.Common.Constants;

namespace Sparkle.Application.Users.Queries.GetUserIdFromUserName
{
    public class GetUserIdFromUserNameQueryValidator : AbstractValidator<GetUserIdFromUserNameQuery>
    {
        public GetUserIdFromUserNameQueryValidator()
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