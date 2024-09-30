using Application.Dtos.UseCases;
using FluentValidation;

namespace Application.Validation.Validators
{
    public class StartMonthUseCaseValidator : AbstractValidator<StartMonthUseCaseDto>
    {
        public StartMonthUseCaseValidator() 
        {
            RuleFor(x => x.EmployeeId)
                .NotNull().WithMessage("EmployeeId is required.")
                .GreaterThan(0).WithMessage("EmployeeId is required.");
        }
    }
}
