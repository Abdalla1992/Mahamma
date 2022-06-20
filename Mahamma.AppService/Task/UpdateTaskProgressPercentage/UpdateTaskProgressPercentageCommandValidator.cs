using FluentValidation;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Notification.ApiClient.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.UpdateTaskProgressPercentage
{
    public class UpdateTaskProgressPercentageCommandValidator : AbstractValidator<UpdateTaskProgressPercentageCommand>
    {
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        public UpdateTaskProgressPercentageCommandValidator(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext,
            IMessageResourceReader messageResourceReader)
        {
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;
            RuleFor(command => command.TaskId).GreaterThan(0).WithMessage(GetValidationMessage("ValidTaskId"));
            RuleFor(command => command.ProgressPercentage).GreaterThan(0).WithMessage(GetValidationMessage("InvalidProgressPercentage"));
            RuleFor(command => command.ProgressPercentage).LessThanOrEqualTo(100).WithMessage(GetValidationMessage("InvalidProgressPercentageGreaterThanOneHundred"));
        }
        private string GetValidationMessage(string key)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            var message = _messageResourceReader.GetKeyValue(key, currentUser != null ? currentUser.LanguageId : Language.English.Id);
            return message;
        }
    }
}
