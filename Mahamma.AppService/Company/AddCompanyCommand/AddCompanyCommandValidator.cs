using FluentValidation;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Company.Repositroy;
using Mahamma.Identity.ApiClient.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Company.AddCompany
{
    class AddCompanyCommandValidator : AbstractValidator<AddCompanyCommand>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        public AddCompanyCommandValidator(ICompanyRepository companyRepository,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _companyRepository = companyRepository;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;

            //RuleFor(command => command.Name).NotEmpty().WithMessage(GetValidationMessage("NameIsEmpty"));
            //RuleFor(command => command).Must(IsUniqueCompany).WithMessage(GetValidationMessage("NameAlreadyExists"));
        }

        public bool IsUniqueCompany(AddCompanyCommand addCompanyCommand)
        {
            return !(_companyRepository.CheckCompanyExistence(addCompanyCommand.Name, 0).Result);
        }
        private string GetValidationMessage(string Key)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            var message = _messageResourceReader.GetKeyValue(Key, currentUser.LanguageId);
            return message;
        }

    }
}
