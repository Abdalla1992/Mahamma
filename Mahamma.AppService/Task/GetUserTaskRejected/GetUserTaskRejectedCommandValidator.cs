using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.GetUserTaskRejected
{
    class GetUserTaskRejectedCommandValidator : AbstractValidator<GetUserTaskRejectedCommand>
    {
        public GetUserTaskRejectedCommandValidator()
        {
            RuleFor(command => command.UserId).GreaterThan(0).WithMessage("User Id Is Less than 1");
        }
    }
}
