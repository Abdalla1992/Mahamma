using FluentValidation;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Task.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.AddComment
{
    public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
    {
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        public AddCommentCommandValidator(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;
            RuleFor(command => command.TaskId).GreaterThan(0).WithMessage(GetValidationMessage("ValidTaskId"));
            RuleFor(command => command.Comment).NotEmpty().WithMessage(GetValidationMessage("InvalidTaskName"));
        }

        private string GetValidationMessage(string Key)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            var message = _messageResourceReader.GetKeyValue(Key, currentUser.LanguageId);
            return message;
        }
    }
}
