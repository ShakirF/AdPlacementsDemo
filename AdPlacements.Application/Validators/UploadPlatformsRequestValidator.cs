using AdPlacements.Application.Contracts;
using FluentValidation;

namespace AdPlacements.Application.Validators
{
    public sealed class UploadPlatformsRequestValidator: AbstractValidator<UploadPlatformsRequest>
    {
        public UploadPlatformsRequestValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("File is required")
                .Must(f => f.Length > 0).WithMessage("File must not be empty");
        }
    }
}
