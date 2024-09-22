using Application.Dtos.UseCases;
using FluentValidation;

namespace Application.Validation.Validators
{
    public class RegisterEntryUseCaseDtoValidator : AbstractValidator<RegisterEntryUseCaseDto>
    {
        public RegisterEntryUseCaseDtoValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotNull().WithMessage("EmployeeId is required.")
                .GreaterThan(0).WithMessage("EmployeeId is required.");

            RuleFor(x => x.PlateNumber)
                .NotNull().WithMessage("PlateNumber is required.")
                .NotEmpty().WithMessage("PlateNumber is required.");

            RuleFor(x => x.InitialDate)
                .NotNull().WithMessage("InitialDate is required");
        }
    }
}
