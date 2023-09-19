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
            RuleFor(c => c.ExpireTime).NotNull();
            RuleFor(c => c.ExpireTime).GreaterThan(DateTime.UtcNow);
            RuleFor(c => c.ExpireTime).LessThan(DateTime.UtcNow.AddDays(Constants.Server.MaxInvetationLiveDays));
        }
    }
}
