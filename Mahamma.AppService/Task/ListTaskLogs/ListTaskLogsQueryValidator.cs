using FluentValidation;

namespace Mahamma.AppService.Task.ListTaskLogs
{
    public class ListTasklogsQueryValidator : AbstractValidator<ListTaskLogsQuery>
    {
        public ListTasklogsQueryValidator()
        {
            RuleFor(command => command.TaskId).GreaterThan(0).WithMessage("Task Id Is Less than 1");
        }
    }
}
