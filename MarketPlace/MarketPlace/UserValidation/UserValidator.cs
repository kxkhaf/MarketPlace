using FluentValidation;

namespace MarketPlace;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Name).NotEmpty()
            .MinimumLength(2)
            .Matches(@"^[\S]+$").WithMessage("Invalid name! The name mustn't contain spaces");
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches("[A-Z]+").WithMessage("'{PropertyName}' must contain one or more capital letters.")
            .Matches("[a-z]+").WithMessage("'{PropertyName}' must contain one or more lowercase letters.")
            .Matches(@"(\d)+").WithMessage("'{PropertyName}' must contain one or more digits.")
            .Matches(@"[""!@$%^&*(){}:;<>,.?/+\-_=|'[\]~\\]")
            .WithMessage("'{ PropertyName}' must contain one or more special characters.");
    }
}

public class NameValidator : AbstractValidator<string>
{
    public NameValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .MinimumLength(2)
            .Matches(@"^[\S]+$").WithMessage("Invalid name! The name mustn't contain spaces");
    }
}
public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .MinimumLength(8)
            .Matches("[A-Z]+").WithMessage("'{PropertyName}' must contain one or more capital letters.")
            .Matches("[a-z]+").WithMessage("'{PropertyName}' must contain one or more lowercase letters.")
            .Matches(@"(\d)+").WithMessage("'{PropertyName}' must contain one or more digits.")
            .Matches(@"[""!@$%^&*(){}:;<>,.?/+\-_=|'[\]~\\]").WithMessage("'{ PropertyName}' must contain one or more special characters.")
            .Matches("(?!.*[£# “”])").WithMessage("'{PropertyName}' must not contain the following characters £ # “” or spaces.")
            .WithMessage("'{PropertyName}' contains a word that is not allowed.");
    }
}