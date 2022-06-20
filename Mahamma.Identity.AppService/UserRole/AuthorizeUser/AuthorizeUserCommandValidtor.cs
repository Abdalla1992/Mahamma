using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.UserRole.AuthorizeUser
{
    public class AuthorizeUserCommandValidtor : AbstractValidator<AuthorizeUserCommand>
    {
        public AuthorizeUserCommandValidtor()
        {
            RuleFor(command => command.UserId).GreaterThan(0).WithMessage("User Id is less than zero");
            RuleFor(command => command.PermissionId).GreaterThan(0).WithMessage("Permission Id is less than zero");
            RuleFor(command => command.PageId).GreaterThan(0).WithMessage("Page Id is less than zero");
        }
    }
}
