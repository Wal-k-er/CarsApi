using Cars.Dto;
using FluentValidation;

namespace Cars.Validators;

public class ManufactureValidator : AbstractValidator<ManufactureDto>
{
    public ManufactureValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().MinimumLength(1).WithMessage("Наименование должно быть");
        RuleFor(c => c.Itn)
            .NotEmpty().MinimumLength(1).WithMessage("ИНН должен быть");
        RuleFor(c => c.Psrn)
            .NotEmpty().MinimumLength(1).WithMessage("ОГРН должен быть");
    }
}