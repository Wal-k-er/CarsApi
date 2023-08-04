using Cars.Dto;
using FluentValidation;

namespace Cars.Validators;

public class CarValidator : AbstractValidator<CarDto>
{
    public CarValidator()
    {
        var past = DateTime.Today.AddYears(-15);
        RuleFor(c => c.Name)
            .NotEmpty().MinimumLength(1).WithMessage("Наименование обязательно")
            .NotEqual("").WithMessage("Наименование обязательно");
        RuleFor(c => c.DateProduction)
            .NotEmpty().WithMessage("Дата начала выпуска обязательна")
            .GreaterThan(past).WithMessage("Дата выпуска должна быть в пределах 15 лет");
        RuleFor(c => c.Price)
            .NotEmpty().WithMessage("Стоимость обязательна")
            .GreaterThan(0).WithMessage("Значение должно быть не отрицательным");
    }
}