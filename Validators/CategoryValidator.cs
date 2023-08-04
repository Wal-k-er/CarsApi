using Cars.Dto;
using FluentValidation;

namespace Cars.Validators;

public class CategoryValidator : AbstractValidator<CategoryDto>
{
    public CategoryValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().MinimumLength(1).WithMessage("Наименование должно быть");
    }
}