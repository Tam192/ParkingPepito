using Application.Dtos.UseCases;
using FluentValidation;

namespace Application.Validation.Validators
{
    public class RegisterResidentVehicleValidator : AbstractValidator<RegisterResidentVehicleDto>
    {
        public RegisterResidentVehicleValidator()
        {
            _ = RuleFor(x => x.PlateNumber)
                    .NotNull().WithMessage("PlateNumber is required.")
                    .NotEmpty().WithMessage("PlateNumber is required.");
        }
    }
}
