using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Sparkle.Application.Common.Constants;

namespace Sparkle.Application.Servers.Roles.Common.Validation
{
    public class ClaimValidator : AbstractValidator<IdentityRoleClaim<Guid>>
    {
        public ClaimValidator()
        {
            RuleFor(c => c.ClaimType).NotNull().NotEmpty();
            RuleFor(c => c.ClaimType).Must(type => Constants.Claims.GetClaims().Contains(type))
                .WithMessage("Claim '{PropertyValue}' does not exist");

            RuleFor(c => c.ClaimValue).NotNull().NotEmpty();
        }
    }

}