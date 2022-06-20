using FluentValidation;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Domain.Task.Enum;
using Mahamma.Domain.Task.Repository;
using System.Linq;

namespace Mahamma.AppService.Task.AddSubTask
{
    public class AddSubTaskCommandValidator : AbstractValidator<AddSubTaskCommand>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        public AddSubTaskCommandValidator(ITaskRepository taskRepository, IProjectRepository projectRepository)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            RuleFor(command => command.ProjectId).GreaterThan(0).WithMessage("Invalid ProjectId");
            RuleFor(command => command.ParentTaskId).GreaterThan(0).WithMessage("Invalid ParentTaskId");
            RuleFor(command => command.Name).NotEmpty().WithMessage("Task Name Is Empty");
            RuleFor(command => command).Must(ValidPriority).WithMessage("Invalid Task Priority");
            RuleFor(command => command).Must(IsUniqueTask).WithMessage("Task Already Exists");
            RuleFor(command => command).Must(ValidEndDate).WithMessage("Invalid Due Date");
            RuleFor(command => command).Must(ValidDateForProject).WithMessage("Task duration is out of porject duration limit");
        }

        private bool ValidPriority(AddSubTaskCommand addTaskCommand)
        {
            return TaskPriority.List().Any(e => e.Id == addTaskCommand.TaskPriorityId);
        }
        private bool IsUniqueTask(AddSubTaskCommand addTaskCommand)
        {
            return !(_taskRepository.CheckSubtaskExistence(addTaskCommand.Name, addTaskCommand.ParentTaskId).Result);
        }
        private bool ValidEndDate(AddSubTaskCommand addTaskCommand)
        {
            return addTaskCommand.DueDate.Date >= addTaskCommand.StartDate.Date;
        }
        private bool ValidDateForProject(AddSubTaskCommand addTaskCommand)
        {
            return (_projectRepository.ValidDate(addTaskCommand.StartDate, addTaskCommand.DueDate, addTaskCommand.ProjectId).Result);
        }
    }
}
