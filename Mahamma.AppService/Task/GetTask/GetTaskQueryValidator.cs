using FluentValidation;
using Mahamma.Domain.Task.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.GetTask
{
    public class GetTaskQueryValidator : AbstractValidator<GetTaskQuery>
    {
        public GetTaskQueryValidator()
        {
            RuleFor(command => command.Id).GreaterThan(0).WithMessage("Task Id Is Less than 1");

        }
    }
}
