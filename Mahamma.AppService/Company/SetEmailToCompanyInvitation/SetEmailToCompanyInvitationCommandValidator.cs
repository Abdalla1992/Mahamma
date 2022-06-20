using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Company.SetEmailToCompanyInvitation
{
    public class SetEmailToCompanyInvitationCommandValidator : AbstractValidator<SetEmailToCompanyInvitationCommand>
    {
        public SetEmailToCompanyInvitationCommandValidator()
        {
            RuleFor(command => command.Id).GreaterThan(0).WithMessage("Company invitation id is less than zero");
            RuleFor(command => command.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(command => command.Email).EmailAddress().WithMessage("Email is invalid");
        }
    }
}
