using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.PagePermissionByRole.GetPagePermissionByRole
{
    public class GetPagePermissionByRoleCommandValidator : AbstractValidator<GetPagePermissionByRoleCommand>
    {
        public GetPagePermissionByRoleCommandValidator()
        {
            //RuleFor(command => command.currentUserId).NotEmpty().WithMessage("CurrentUserId Is Required");
            RuleFor(command => command.currentRoleId).NotEmpty().WithMessage("CurrentRoleId Is Required");
        }
    }
}
