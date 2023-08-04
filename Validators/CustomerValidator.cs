using Cars.Dto;
using FluentValidation;

namespace Cars.Validators;

public class CustomerValidator : AbstractValidator<CustomerDto>
{
    public CustomerValidator()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty().MinimumLength(1).WithMessage("Имя обязательно")
            .MaximumLength(50).WithMessage("Максимальное количество знаков 50");
        RuleFor(c => c.LastName)
            .NotEmpty().MinimumLength(1).WithMessage("Фамилия обязательна")
            .MaximumLength(50).WithMessage("Максимальное количество знаков 50");
    }
}