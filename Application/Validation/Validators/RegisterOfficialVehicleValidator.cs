using Application.Dtos.UseCases;
using FluentValidation;

namespace Application.Validation.Validators
{
    public class RegisterOfficialVehicleValidator : AbstractValidator<RegisterOfficialVehicleDto>
    {
        public RegisterOfficialVehicleValidator() 
        {
            //RuleFor(x => x.EmployeeId)
            //    .NotNull().WithMessage("EmployeeId is required.")
            //    .GreaterThan(0).WithMessage("EmployeeId is required.");

            RuleFor(x => x.PlateNumber)
                    .NotNull().WithMessage("PlateNumber is required.")
                    .NotEmpty().WithMessage("PlateNumber is required.");
        }
    }
}
