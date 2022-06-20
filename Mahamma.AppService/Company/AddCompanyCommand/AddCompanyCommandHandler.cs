using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Company.Entity;
using Mahamma.Domain.Company.Enum;
using Mahamma.Domain.Company.Repositroy;
using Mahamma.Identity.ApiClient.Dto.Base;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Identity.ApiClient.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Company.AddCompany
{
    class AddCompanyCommandHandler : IRequestHandler<AddCompanyCommand, ValidateableResponse<ApiResponse<int>>>
    {
        #region Prop
        private readonly ICompanyRepository _companyRepository;
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;

        #endregion

        #region Ctor
        public AddCompanyCommandHandler(ICompanyRepository companyRepository, IAccountService accountService,
            IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader, IRoleService roleService)
        {
            _companyRepository = companyRepository;
            _accountService = accountService;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;
            _roleService = roleService;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<int>>> Handle(AddCompanyCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<int>> response = new(new ApiResponse<int>());

            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];

            Domain.Company.Entity.Company company = new();
            List<CompanyInvitation> companyInvitations = new();
            foreach (var email in request.InvitationsEmails)
            {
                CompanyInvitation companyInvitation = new();
                companyInvitation.CreateCompanyInvitations(company.Id, email, currentUser.Id, Guid.NewGuid().ToString(), InvitationStatus.New.Id
                                                            ,null, null, null );
                companyInvitations.Add(companyInvitation);
            }

            company.CreateCompany(request.Name, request.CompanySize, request.Descreption, currentUser.Id, companyInvitations);

            _companyRepository.AddCompany(company);

            if (await _companyRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                bool rolesSetToCompany = await _roleService.SetCompanyBasicRoles(new BaseRequestDto() { AuthToken = GetAccessToken() }, company.Id);
                if (rolesSetToCompany)
                {
                    bool companySetToUser = await _accountService.SetUserCompany(new BaseRequestDto() { AuthToken = GetAccessToken() }, company.Id);
                    if (companySetToUser)
                    {
                        response.Result.ResponseData = company.Id;
                        response.Result.CommandMessage = _messageResourceReader.GetKeyValue("DataAddededSuccessfully",currentUser.LanguageId);
                    }
                    else
                    {
                        response.Result.CommandMessage = _messageResourceReader.GetKeyValue("GeneralError",currentUser.LanguageId);
                    }
                }
                else
                {
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("GeneralError", currentUser.LanguageId);
                }
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToAddTheNewData",currentUser.LanguageId);
            }
            return response;
        }
        private string GetAccessToken()
        {
            string apiToken = string.Empty;
            if (_httpContext.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues value))
                apiToken = value;

            return apiToken;
        }
    }
}
