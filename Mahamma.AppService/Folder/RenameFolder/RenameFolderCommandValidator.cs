using FluentValidation;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Identity.ApiClient.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Folder.RenameFolder
{
    public class RenameFolderCommandValidator : AbstractValidator<RenameFolderCommand>
    {
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;


        public RenameFolderCommandValidator(IMessageResourceReader messageResourceReader,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext)
        {
            _messageResourceReader = messageResourceReader;
            _httpContext = httpContext;
            RuleFor(command => command.Id).GreaterThan(0).WithMessage(GetValidationMessage("InvalidFolderId"));
            RuleFor(command => command.Name).NotEmpty().WithMessage(GetValidationMessage("NameIsEmpty"));
        }

        private string GetValidationMessage(string Key)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            var message = _messageResourceReader.GetKeyValue(Key, currentUser.LanguageId);
            return message;
        }
    }
}
