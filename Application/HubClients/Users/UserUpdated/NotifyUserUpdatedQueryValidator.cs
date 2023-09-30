using FluentValidation;

namespace Sparkle.Application.HubClients.Users.UserUpdated
{
    public class NotifyUserUpdatedQueryValidator : AbstractValidator<NotifyUserUpdatedQuery>
    {
        public NotifyUserUpdatedQueryValidator()
        {
            RuleFor(x => x.UpdatedUser).NotNull();
        }
    }
}
