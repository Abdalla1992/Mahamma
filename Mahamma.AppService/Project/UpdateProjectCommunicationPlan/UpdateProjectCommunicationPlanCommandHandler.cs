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

namespace Mahamma.AppService.Project.UpdateProjectCommunicationPlan
{
    public class UpdateProjectCommunicationPlanCommandHandler : IRequestHandler<UpdateProjectCommunicationPlanCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly IProjectCommunicationPlanRepository _projectCommunicationPlanRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IProjectActivityLogger _projectActivityLogger;
        private readonly ActivitesSettings _activitesSettings;
        #endregion

        #region Ctor
        public UpdateProjectCommunicationPlanCommandHandler(IProjectCommunicationPlanRepository projectCommunicationPlanRepository, IMessageResourceReader messageResourceReader
                                                    , IHttpContextAccessor httpContext, IProjectActivityLogger projectActivityLogger,
                                                    ActivitesSettings activitesSettings)
        {
            _projectCommunicationPlanRepository = projectCommunicationPlanRepository;
            _messageResourceReader = messageResourceReader;
            _httpContext = httpContext;
            _projectActivityLogger = projectActivityLogger;
            _activitesSettings = activitesSettings;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(UpdateProjectCommunicationPlanCommand request, CancellationToken cancellationToken)
        {
            UserDto currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());

            ProjectCommunicationPlanDto projectCommunicationPlanDto = new ProjectCommunicationPlanDto()
            {
                Id = request.Id,
                ProjectId = request.ProjectId,
                CommunicationType = request.CommunicationType,
                DeliveryMethod = request.DeliveryMethod,
                Frequency = request.Frequency,
                Owner = request.Owner,
                Goal = request.Goal,
                KeyDates = request.KeyDates,
                Notes = request.Notes,
                Recipient = request.Recipient,
                ResourceLinks = request.ResourceLinks
            };

            _projectCommunicationPlanRepository.UpdateProjectCommunicationPlan(projectCommunicationPlanDto);

            if (await _projectCommunicationPlanRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                await _projectActivityLogger.LogProjectActivity(_activitesSettings.UpdateProjectCommunicationPlan, projectCommunicationPlanDto.ProjectId, currentUser.Id, cancellationToken);
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
