using Promomash.Application.Features.Users.CreateUser;

namespace Promomash.Application.Features.Users.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MaximumLength(100).WithMessage("Password is to large")
            .Must(ContainAtLeastOneDigit).WithMessage("Password must contain at least one digit.")
            .Must(ContainAtLeastOneLetter).WithMessage("Password must contain at least one letter.");

        RuleFor(p => p.CountryId)
            .NotEmpty();
        RuleFor(p => p.ProvinceId)
            .NotEmpty();
    }

    private bool ContainAtLeastOneDigit(string password)
    {
        return password.Any(char.IsDigit);
    }

    private bool ContainAtLeastOneLetter(string password)
    {
        return password.Any(char.IsLetter);
    }
}