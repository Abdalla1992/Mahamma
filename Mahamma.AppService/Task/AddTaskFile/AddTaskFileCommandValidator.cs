using FluentValidation;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Identity.ApiClient.Dto.User;
using System;

namespace Mahamma.AppService.Task.AddTaskFile
{
    public class AddTaskFileCommandValidator : AbstractValidator<AddTaskFileCommand>
    {
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        public AddTaskFileCommandValidator(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;
            RuleFor(command => command.ProjectId).GreaterThan(0).WithMessage(GetValidationMessage("InvalidProjectId"));
            RuleFor(command => command.TaskId).Must(ToBeOrNotToBe).WithMessage(GetValidationMessage("ValidTaskId"));
        }

        private bool ToBeOrNotToBe(int? taskId)
        {
            return taskId == null || taskId > 0;
        }
        private string GetValidationMessage(string Key)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            var message = _messageResourceReader.GetKeyValue(Key, currentUser.LanguageId);
            return message;
        }
    }
}
