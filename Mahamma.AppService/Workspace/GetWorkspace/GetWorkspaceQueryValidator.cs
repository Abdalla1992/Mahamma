using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Workspace.GetWorkspace
{
    public class GetWorkspaceQueryValidator : AbstractValidator<GetWorkspaceQuery>
    {
        public GetWorkspaceQueryValidator()
        {
            RuleFor(command => command.Id).GreaterThan(0).WithMessage("Workspace Id Is Less than 1");
        }
    }
}
