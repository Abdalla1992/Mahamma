using FluentValidation;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Task.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.ArchiveTaskList
{
    public class ArchiveTaskListCommandValidator : AbstractValidator<ArchiveTaskListCommand>
    {
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        public ArchiveTaskListCommandValidator( Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;
            RuleFor(command => command.TaskIdList).Must(AllHaveValue).WithMessage(GetValidationMessage("ValidTaskIdList"));

        }
        private bool AllHaveValue(List<int> taskList)
        {
            return taskList.Count() > 0 &&  taskList.All(t => t > 0);
        }

        private string GetValidationMessage(string Key)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            var message = _messageResourceReader.GetKeyValue(Key, currentUser.LanguageId);
            return message;
        }
    }
}
