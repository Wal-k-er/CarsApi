using Cars.Dto;
using FluentValidation;

namespace Cars.Validators;

public class OrderValidator : AbstractValidator<OrderDto>
{
    public OrderValidator()
    {
        RuleFor(o => o.CreatedDate)
            .GreaterThan(DateTime.Today).WithMessage("Заказ должен быть создан не раньше чем сегодня");
    }
}