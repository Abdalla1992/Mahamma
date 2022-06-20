using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Role.UpdateRole
{
   public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            RuleFor(command => command.Id).GreaterThan(0).WithMessage("Role Id Is  Less than 1");
            RuleFor(command => command.Name).NotEmpty().WithMessage("Role Name Is Required");
        }
    }
}
