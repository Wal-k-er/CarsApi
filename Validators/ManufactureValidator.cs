using Cars.Dto;
using FluentValidation;

namespace Cars.Validators;

public class ManufactureValidator : AbstractValidator<ManufactureDto>
{
    public ManufactureValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Наименование должно быть");
        RuleFor(c => c.Itn)
            .NotEmpty().WithMessage("ИНН должен быть");
        RuleFor(c => c.Psrn)
            .NotEmpty().WithMessage("ОГРН должен быть");
    }
}