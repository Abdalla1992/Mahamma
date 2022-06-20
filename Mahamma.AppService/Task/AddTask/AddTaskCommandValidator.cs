using FluentValidation;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Domain.Task.Enum;
using Mahamma.Domain.Task.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.AddTask
{
    public class AddTaskCommandValidator : AbstractValidator<AddTaskCommand>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        public AddTaskCommandValidator(ITaskRepository taskRepository, IProjectRepository projectRepository,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;

            RuleFor(command => command.ProjectId).GreaterThan(0).WithMessage(GetValidationMessage("InvalidProjectId"));
            RuleFor(command => command.Name).NotEmpty().WithMessage(GetValidationMessage("InvalidTaskName"));
            RuleFor(command => command).Must(IsUniqueTask).WithMessage(GetValidationMessage("NameAlreadyExists"));
            RuleFor(command => command).Must(ValidPriority).WithMessage(GetValidationMessage("InvalidTaskPriority"));
            RuleFor(command => command).Must(IsUniqueTask).WithMessage(GetValidationMessage("NameAlreadyExists"));
            RuleFor(command => command).Must(ValidEndDate).WithMessage(GetValidationMessage("InvalidDueDate"));
            RuleFor(command => command).Must(ValidDateForProject).WithMessage(GetValidationMessage("InvalidTaskdurationWithProject"));
        }

        private bool ValidPriority(AddTaskCommand addTaskCommand)
        {
            return TaskPriority.List().Any(e => e.Id == addTaskCommand.TaskPriorityId);
        }

        private bool IsUniqueTask(AddTaskCommand addTaskCommand)
        {
            return !(_taskRepository.CheckTaskExistence(addTaskCommand.Name, addTaskCommand.ProjectId).Result);
        }
        private bool ValidEndDate(AddTaskCommand addTaskCommand)
        {
            return addTaskCommand.DueDate.Date >= addTaskCommand.StartDate.Date;
        }
        private bool ValidDateForProject(AddTaskCommand addTaskCommand)
        {
            return (_projectRepository.ValidDate(addTaskCommand.StartDate, addTaskCommand.DueDate, addTaskCommand.ProjectId).Result);
        }

        private string GetValidationMessage(string Key)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            var message = _messageResourceReader.GetKeyValue(Key, currentUser.LanguageId);
            return message;
        }
    }
}
