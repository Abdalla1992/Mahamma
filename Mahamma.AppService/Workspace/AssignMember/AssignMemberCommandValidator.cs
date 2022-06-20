using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Workspace.AssignMember
{
    public class AssignMemberCommandValidator : AbstractValidator<AssignWorkSpaceMemberCommand>
    {
        public AssignMemberCommandValidator()
        {
            RuleFor(command => command.WorkSpaceId).GreaterThan(0).WithMessage("Workspace Id Is Less than 1");
            RuleFor(command => command.UserIds).Must(AllHaveValue).WithMessage("UserIdList Is Not Valid");
        }

        private bool AllHaveValue(List<long> userList)
        {
            return userList.Count() > 0 && userList.All(u => u > 0);
        }
    }
}
