using AdPlacements.Application.Contracts;
using FluentValidation;

namespace AdPlacements.Application.Validators
{
    public sealed class SearchPlatformsQueryValidator: AbstractValidator<SearchPlatformsQuery>
    {
        public SearchPlatformsQueryValidator()
        {
            RuleFor(x => x.Location)
                .NotEmpty()
                .Must(l => l.StartsWith('/')).WithMessage("Location must start with '/'");
        }
    }
}
