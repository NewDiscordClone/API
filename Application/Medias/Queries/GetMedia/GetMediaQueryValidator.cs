using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Medias.Queries.GetMedia
{
    public class GetMediaQueryValidator : AbstractValidator<GetMediaQuery>
    {
        public GetMediaQueryValidator()
        {
            RuleFor(q => q.Id).NotNull().IsObjectId();
        }
    }
}
