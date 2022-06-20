using FluentValidation;
using Mahamma.Identity.Domain.Role.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Role.SetCompanyBasicRoles
{
    class SetCompanyBasicRolesCommandValidator : AbstractValidator<SetCompanyBasicRolesCommand>
    {
        public SetCompanyBasicRolesCommandValidator()
        {
            RuleFor(command => command.CompanyId).GreaterThan(0).WithMessage("Company Id Is Empty");
        }
    }
}
