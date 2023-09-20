using FluentValidation;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.RegularExpressions;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Roles.Commands.Create
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(c => c.Name).RequiredMaximumLength(
                Constants.ServerProfile.RoleNameMaxLength);

            RuleFor(c => c.Color).NotNull().NotEmpty();
            RuleFor(c => c.Color).Matches(Regexes.ColorRegex())
                .WithMessage("Color must be in #RRGGBB format");

            RuleFor(c => c.ServerId).NotNull().IsObjectId();

            RuleFor(c => c.Priority).NotNull().GreaterThanOrEqualTo(0);

            RuleFor(c => c.Claims).NotNull();
        }
    }
}
