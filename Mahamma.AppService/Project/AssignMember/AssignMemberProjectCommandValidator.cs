using FluentValidation;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Identity.ApiClient.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.AssignMember
{
    public class AssignMemberProjectCommandValidator : AbstractValidator<AssignMemberProjectCommand>
    {
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        public AssignMemberProjectCommandValidator(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;
            RuleFor(command => command.ProjectId).GreaterThan(0).WithMessage(GetValidationMessage("InvalidProjectId"));
            RuleFor(command => command.UserIdList).Must(AllHaveValue).WithMessage(GetValidationMessage("InvalidUserIdList"));
        }
        private bool AllHaveValue(List<long> userList)
        {
            return userList.Count() > 0 && userList.All(u => u > 0);
        }
        private string GetValidationMessage(string Key)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            var message = _messageResourceReader.GetKeyValue(Key, currentUser.LanguageId);
            return message;
        }

    }
}
