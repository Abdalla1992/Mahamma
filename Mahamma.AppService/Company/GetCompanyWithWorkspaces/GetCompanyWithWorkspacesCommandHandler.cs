using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.Company.Dto;
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

namespace Mahamma.AppService.Company.GetCompanyWithWorkspaces
{
    public class GetCompanyWithWorkspacesCommandHandler : IRequestHandler<SearchCompanyDetailsDto, PageList<CompanyDetailsDto>>
    {
        #region Prop
        private readonly ICompanyReadRepository _companyReadRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly AppSetting _appSetting;

        #endregion

        #region Ctor
        public GetCompanyWithWorkspacesCommandHandler(ICompanyReadRepository companyReadRepository, IHttpContextAccessor httpContext, AppSetting appSetting)
        {
            _companyReadRepository = companyReadRepository;
            _httpContext = httpContext;
            _appSetting = appSetting;
        }
        #endregion

        public async Task<PageList<CompanyDetailsDto>> Handle(SearchCompanyDetailsDto request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            request.Filter.CreatorUserId = currentUser.Id;
            request.Filter.Id = currentUser.CompanyId;

            return await _companyReadRepository.GetCompanyWithWorkspaces(request, currentUser.CompanyId, currentUser.Id, currentUser.RoleName, _appSetting.SuperAdminRole);
        }
    }
}

