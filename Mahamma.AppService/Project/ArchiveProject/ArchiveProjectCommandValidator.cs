using FluentValidation;
using Mahamma.AppService.Task.ArchiveTask;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Identity.ApiClient.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.ArchiveProject
{
  public class ArchiveProjectCommandValidator : AbstractValidator<ArchiveProjectCommand>
    {
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        public ArchiveProjectCommandValidator(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;
            RuleFor(m => m.Id).GreaterThan(0).WithMessage(GetValidationMessage("InvalidProjectId"));

        }
        private string GetValidationMessage(string Key)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            var message = _messageResourceReader.GetKeyValue(Key, currentUser.LanguageId);
            return message;
        }
    }
}
