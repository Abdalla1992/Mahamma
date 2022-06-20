using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Account.ForgetPassword
{
    public class ForgetPasswordCommandValidator : AbstractValidator<ForgetPasswordCommand>
    {
        public ForgetPasswordCommandValidator()
        {
            RuleFor(command => command.Email).NotEmpty().WithMessage("Email cannot be empty");
            RuleFor(command => command.Email).EmailAddress().WithMessage("Invalid email format");
        }
    }
}
