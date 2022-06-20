using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.Domain.PagePermissionByRoleId.Dto;
using Mahamma.Identity.Domain.PagePermissionByRoleId.Repository;
using Mahamma.Identity.Domain.User.Dto;
using Mahamma.Identity.Domain.User.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.PagePermissionByRole.GetPagePermissionByRole
{
    public class GetPagePermissionByRoleCommandHandler : IRequestHandler<GetPagePermissionByRoleCommand, ValidateableResponse<ApiResponse<List<PagePermissionLocalizationDto>>>>
    {
        #region Prop
        private readonly IPagePermissionLocalizationRepository _pagePermissionByRoleRepository;
        private readonly IHttpContextAccessor _httpContext;


        #endregion

        #region Ctor
        public GetPagePermissionByRoleCommandHandler(IPagePermissionLocalizationRepository pagePermissionByRoleRepository, IHttpContextAccessor httpContext)
        {
            _pagePermissionByRoleRepository = pagePermissionByRoleRepository;
            _httpContext = httpContext;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<List<PagePermissionLocalizationDto>>>> Handle(GetPagePermissionByRoleCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<List<PagePermissionLocalizationDto>>>  response = new(new ApiResponse<List<PagePermissionLocalizationDto>>());
            long userId = (long)_httpContext.HttpContext.Items["UserId"];

            List<PagePermissionLocalizationDto> result =await _pagePermissionByRoleRepository.GetPagePermissionByRoleId(userId, request.currentRoleId);
          
            response.Result.ResponseData = result;
            return response;
        }
    }
}
