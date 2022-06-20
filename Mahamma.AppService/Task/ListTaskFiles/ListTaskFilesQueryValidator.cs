using FluentValidation;

namespace Mahamma.AppService.Task.ListTaskFiles
{
    public class ListTaskFilesQueryValidator : AbstractValidator<ListTaskFilesQuery>
    {
        public ListTaskFilesQueryValidator()
        {
            RuleFor(command => command.TaskId).GreaterThan(0).WithMessage("Task Id Is Less than 1");
        }
    }
}
