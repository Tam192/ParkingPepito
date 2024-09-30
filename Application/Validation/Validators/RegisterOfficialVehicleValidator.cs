using Application.Dtos.UseCases;
using FluentValidation;

namespace Application.Validation.Validators
{
    public class RegisterOfficialVehicleValidator : AbstractValidator<RegisterOfficialVehicleDto>
    {
        public RegisterOfficialVehicleValidator() 
        {
            RuleFor(x => x.PlateNumber)
                    .NotNull().WithMessage("PlateNumber is required.")
                    .NotEmpty().WithMessage("PlateNumber is required.");
        }
    }
}
