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

namespace Mahamma.AppService.Project.GetProjectCommunicationPlans
{
    public class GetProjectCommunicationPlansQueryHandler : IRequestHandler<GetProjectCommunicationPlansQuery, ValidateableResponse<ApiResponse<List<ProjectCommunicationPlanDto>>>>
    {
        #region Prop
        private readonly IProjectCommunicationPlanRepository _projectCommunicationPlanRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly IHttpContextAccessor _httpContext;
        #endregion

        #region Ctor
        public GetProjectCommunicationPlansQueryHandler(IProjectCommunicationPlanRepository projectCommunicationPlanRepository, IMessageResourceReader messageResourceReader
                                                , IHttpContextAccessor httpContext)
        {
            _projectCommunicationPlanRepository = projectCommunicationPlanRepository;
            _messageResourceReader = messageResourceReader;
            _httpContext = httpContext;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<List<ProjectCommunicationPlanDto>>>> Handle(GetProjectCommunicationPlansQuery request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<List<ProjectCommunicationPlanDto>>> response = new(new ApiResponse<List<ProjectCommunicationPlanDto>>());

            List<ProjectCommunicationPlanDto> projectCommunicationPlans = await _projectCommunicationPlanRepository.GetAllProjectCommunicationPlans(request.ProjectId);

            if (projectCommunicationPlans.Count > 0)
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("ProcessCompletedSuccessfully", currentUser.LanguageId);
                response.Result.ResponseData = projectCommunicationPlans;
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("NoDataFound", currentUser.LanguageId);
            }

            return response;
        }
    }
}
