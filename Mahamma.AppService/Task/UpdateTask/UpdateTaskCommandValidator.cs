using FluentValidation;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Task.Repository;
using Mahamma.Identity.ApiClient.Dto.User;

namespace Mahamma.AppService.Task.UpdateTask
{
    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        public UpdateTaskCommandValidator(ITaskRepository taskRepository ,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _taskRepository = taskRepository;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;

            RuleFor(command => command.Id).GreaterThan(0).WithMessage(GetValidationMessage("ValidTaskId"));
            RuleFor(command => command.Name).NotEmpty().WithMessage(GetValidationMessage("NameIsEmpty"));
            RuleFor(command => command).Must(IsUniqueTask).WithMessage(GetValidationMessage("NameAlreadyExists"));
            RuleFor(command => command).Must(ValidEndDate).WithMessage(GetValidationMessage("InvalidDueDate"));
        }
        private bool IsUniqueTask(UpdateTaskCommand updateTaskCommand)
        {
            return !(_taskRepository.CheckUpdateTaskExistence(updateTaskCommand.Name, updateTaskCommand.Id).Result);
        }
        private bool ValidEndDate(UpdateTaskCommand updateTaskCommand)
        {
            return updateTaskCommand.DueDate > updateTaskCommand.StartDate;
        }

        private string GetValidationMessage(string key)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            var message = _messageResourceReader.GetKeyValue(key, currentUser.LanguageId);
            return message;
        }
    }
}
