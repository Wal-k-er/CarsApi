using Cars.Dto;
using FluentValidation;

namespace Cars.Validators;

public class CategoryValidator : AbstractValidator<CategoryDto>
{
    public CategoryValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Наименование должно быть");
    }
}