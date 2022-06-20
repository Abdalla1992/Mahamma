using FluentValidation;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Domain.Task.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.ReviewTask
{
    public class ReviewTaskCommandValidator : AbstractValidator<ReviewTaskCommand>
    {
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        public ReviewTaskCommandValidator(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;
            RuleFor(command => command.TaskId).GreaterThan(0).WithMessage(GetValidationMessage("ValidTaskId"));
        }

        private string GetValidationMessage(string key)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            var message = _messageResourceReader.GetKeyValue(key, currentUser.LanguageId);
            return message;
        }
    }
}
