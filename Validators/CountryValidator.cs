using Cars.Dto;
using FluentValidation;

namespace Cars.Validators;

public class CountryValidator : AbstractValidator<CountryDto>
{
    public CountryValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().MinimumLength(1).WithMessage("Наименование должно быть");
    }
}