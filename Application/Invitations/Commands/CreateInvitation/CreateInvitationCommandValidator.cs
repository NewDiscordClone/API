using FluentValidation;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Invitations.Commands.CreateInvitation
{
    public class CreateInvitationCommandValidator : AbstractValidator<CreateInvitationCommand>
    {
        public CreateInvitationCommandValidator()
        {
            RuleFor(c => c.ServerId).NotNull().IsObjectId();

            RuleFor(c => c.ExpireTime).GreaterThan(DateTime.UtcNow);

            RuleFor(c => c.ExpireTime).LessThan(DateTime.UtcNow.AddDays(Constants.Server.MaxInvetationLiveDays))
                .WithMessage((command, expireTime) =>
                {
                    TimeSpan expireDuration = (expireTime! - DateTime.UtcNow).Value;

                    return $"You can't create an invitation that expires longer than" +
                        $" {Constants.Server.MaxInvetationLiveDays} days. This invitation expires in {expireDuration.Days} days, " +
                        $"{expireDuration.Hours} hours.";
                });
        }
    }
}
