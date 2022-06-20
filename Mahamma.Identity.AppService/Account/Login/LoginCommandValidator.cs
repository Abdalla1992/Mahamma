using FluentValidation;

namespace Mahamma.Identity.AppService.Account.Login.LoginCommand
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(command => command.Email).NotEmpty().WithMessage("Email Is Empty");
        }
    }
}
