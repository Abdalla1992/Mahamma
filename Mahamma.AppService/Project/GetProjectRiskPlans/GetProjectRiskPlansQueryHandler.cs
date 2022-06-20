using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Project.Dto;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.GetProjectRiskPlans
{
    public class GetProjectRiskPlansQueryHandler : IRequestHandler<GetProjectRiskPlansQuery, ValidateableResponse<ApiResponse<List<ProjectRiskPlanDto>>>>
    {
        #region Prop
        private readonly IProjectRiskPlanRepository _projectRiskPlanRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly IHttpContextAccessor _httpContext;
        #endregion

        #region Ctor
        public GetProjectRiskPlansQueryHandler(IProjectRiskPlanRepository projectRiskPlanRepository, IMessageResourceReader messageResourceReader
                                                ,IHttpContextAccessor httpContext)
        {
            _projectRiskPlanRepository = projectRiskPlanRepository;
            _messageResourceReader = messageResourceReader;
            _httpContext = httpContext;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<List<ProjectRiskPlanDto>>>> Handle(GetProjectRiskPlansQuery request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<List<ProjectRiskPlanDto>>> response = new(new ApiResponse<List<ProjectRiskPlanDto>>());

            List<ProjectRiskPlanDto> projectRiskPlans = await _projectRiskPlanRepository.GetAllProjectRiskPlans(request.ProjectId);

            if (projectRiskPlans.Count > 0)
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("ProcessCompletedSuccessfully", currentUser.LanguageId);
                response.Result.ResponseData = projectRiskPlans;
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("NoDataFound", currentUser.LanguageId);
            }

            return response;
        }
    }
}
