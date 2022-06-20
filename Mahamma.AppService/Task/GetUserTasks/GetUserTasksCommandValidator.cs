using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.GetUserTasks
{
    class GetUserTasksCommandValidator : AbstractValidator<GetUserTasksCommand>
    {
        public GetUserTasksCommandValidator()
        {
            RuleFor(command => command.UserId).GreaterThan(0).WithMessage("User Id Is Less than 1");
        }
    }
}
