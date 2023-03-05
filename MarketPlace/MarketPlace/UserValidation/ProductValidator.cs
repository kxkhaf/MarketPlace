using FluentValidation;

namespace MarketPlace;

public class ProductValidator: AbstractValidator<string>
{
public ProductValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .MinimumLength(1)
            .Matches(@"^[\S]+$").WithMessage("Invalid name! The name mustn't contain spaces");
    }
}