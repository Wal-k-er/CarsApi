using Cars.Dto;
using FluentValidation;

namespace Cars.Validators;

public class CountryValidator : AbstractValidator<CountryDto>
{
    public CountryValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Наименование должно быть");
    }
}