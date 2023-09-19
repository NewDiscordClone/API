using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Invitations.Queries.InvitationDetails
{
    public class InvitationDetailsQueryValidator : AbstractValidator<InvitationDetailsQuery>
    {
        public InvitationDetailsQueryValidator()
        {
            RuleFor(c => c.InvitationId).NotNull().IsObjectId();
        }
    }
}
