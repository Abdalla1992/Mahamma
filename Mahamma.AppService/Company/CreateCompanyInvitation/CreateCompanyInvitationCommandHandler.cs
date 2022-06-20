using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.Company.Dto;
using Mahamma.Domain.Company.Entity;
using Mahamma.Domain.Company.Enum;
using Mahamma.Domain.Company.Repositroy;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Company.CreateCompanyInvitation
{
    public class CreateCompanyInvitationCommandHandler : IRequestHandler<CreateCompanyInvitationCommand, ValidateableResponse<ApiResponse<CompanyInvitationDto>>>
    {
        #region Props
        private readonly ICompanyInvitationRepository _companyInvitationRepository;
        private readonly AppSetting _appSetting;
        private readonly IHttpContextAccessor _httpContext;
        #endregion

        #region Ctor
        public CreateCompanyInvitationCommandHandler(ICompanyInvitationRepository companyInvitationRepository, AppSetting appSetting,
            IHttpContextAccessor httpContext)
        {
            _companyInvitationRepository = companyInvitationRepository;
            _appSetting = appSetting;
            _httpContext = httpContext;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<CompanyInvitationDto>>> Handle(CreateCompanyInvitationCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<CompanyInvitationDto>> response = new(new ApiResponse<CompanyInvitationDto>());
            UserDto currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            CompanyInvitation companyInvitation = new();
            companyInvitation.CreateCompanyInvitations(currentUser.CompanyId, string.Empty, currentUser.Id, Guid.NewGuid().ToString(), InvitationStatus.New.Id
                                                        ,null, null, null);

            _companyInvitationRepository.AddCompanyInvitation(companyInvitation);
            if (await _companyInvitationRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                response.Result.ResponseData = _companyInvitationRepository.MapCompanyInvitationToCompanyInvitationdto(companyInvitation);
                response.Result.ResponseData.InvitationLink = $"{_appSetting.InvitationLink}{companyInvitation.InvitationId}";
                response.Result.CommandMessage = "Data Added Successfully";
            }
            return response;
        }
    }
}
