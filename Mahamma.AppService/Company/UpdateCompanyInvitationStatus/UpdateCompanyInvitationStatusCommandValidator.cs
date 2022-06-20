using FluentValidation;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Identity.ApiClient.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Company.UpdateCompanyInvitationStatus
{
    public class UpdateCompanyInvitationStatusCommandValidator : AbstractValidator<UpdateCompanyInvitationStatusCommand>
    {
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        public UpdateCompanyInvitationStatusCommandValidator(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;
            RuleFor(command => command.InvitationId).NotEmpty().WithMessage(GetValidationMessage("invalidinvitation"));
            RuleFor(command => command.InvitationStatusId).GreaterThan(0).WithMessage(GetValidationMessage("InvalidInvitationStatusId"));
        }
        private string GetValidationMessage(string Key)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            var message = _messageResourceReader.GetKeyValue(Key, currentUser.LanguageId);
            return message;
        }
    }
}
