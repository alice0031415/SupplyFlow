using FluentValidation;

namespace SupplyFlow.Procurement.Application.Needs.Commands;

public sealed class CreateNeedValidator
    : AbstractValidator<CreateNeedCommand>
{
    public CreateNeedValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Quantity)
            .GreaterThan(0);

        RuleFor(x => x.RequiredBy)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow.Date));
    }
}