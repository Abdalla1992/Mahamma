using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Role.DeleteRole
{
   public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidator()
        {
            RuleFor(command => command.RoleId).GreaterThan(0).WithMessage("Role Id Is Less than 1");
        }
    }
}
