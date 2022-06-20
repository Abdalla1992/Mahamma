using FluentValidation;
using Mahamma.AppService.Company.GetCompany;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Notification.ApiClient.Enum;

namespace Mahamma.AppService.Company.GetCompany
{
    public class GetCompanyQueryValidator : AbstractValidator<GetCompanyQuery>
    {
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        public GetCompanyQueryValidator(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;
            RuleFor(command => command.Id).GreaterThan(0).WithMessage(GetValidationMessage("InvalidCompanyId"));
        }
        private string GetValidationMessage(string Key)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            var message = _messageResourceReader.GetKeyValue(Key, currentUser != null ? currentUser.LanguageId : Language.English.Id);
            return message;
        }

    }
}
