using FluentValidation;

public class CreatePropertyValidator : AbstractValidator<CreatePropertyCommand>
{
    public CreatePropertyValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Price).GreaterThan(0);
    }
}