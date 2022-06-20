using Mahamma.AppService.AtivityLogger.Project;
using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Project.Dto;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.AddProjectRiskPlan
{
    public class AddProjectRiskPlanCommandHandler : IRequestHandler<AddProjectRiskPlanCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly IProjectRiskPlanRepository _projectRiskPlanRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IProjectActivityLogger _projectActivityLogger;
        private readonly ActivitesSettings _activitesSettings;
        #endregion

        #region Ctor
        public AddProjectRiskPlanCommandHandler(IProjectRiskPlanRepository projectRiskPlanRepository, IMessageResourceReader messageResourceReader
                                                    , IHttpContextAccessor httpContext, IProjectActivityLogger projectActivityLogger,
                                                    ActivitesSettings activitesSettings)
        {
            _projectRiskPlanRepository = projectRiskPlanRepository;
            _messageResourceReader = messageResourceReader;
            _httpContext = httpContext;
            _projectActivityLogger = projectActivityLogger;
            _activitesSettings = activitesSettings;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(AddProjectRiskPlanCommand request, CancellationToken cancellationToken)
        {
            UserDto currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());

            ProjectRiskPlanDto projectRiskPlanDto = new ProjectRiskPlanDto()
            {
                ProjectId = request.ProjectId,
                Action = request.Action,
                Impact = request.Impact,
                Issue = request.Issue,
                Owner = request.Owner
            };

            _projectRiskPlanRepository.AddProjectRiskPlan(projectRiskPlanDto);

            if (await _projectRiskPlanRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                await _projectActivityLogger.LogProjectActivity(_activitesSettings.AddProjectRiskPlan, projectRiskPlanDto.ProjectId, currentUser.Id, cancellationToken);
                response.Result.ResponseData = true;
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("DataSavedSuccessfully", currentUser.LanguageId);
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToUpdateData", currentUser.LanguageId);
            }

            return response;
        }
    }
}
