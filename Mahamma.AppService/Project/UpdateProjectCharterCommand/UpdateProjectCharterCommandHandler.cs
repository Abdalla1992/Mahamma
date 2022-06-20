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

namespace Mahamma.AppService.Project.UpdateProjectCharterCommand
{
    public class UpdateProjectCharterCommandHandler : IRequestHandler<UpdateProjectCharterCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly IProjectCharterRepository _projectCharterRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IProjectActivityLogger _projectActivityLogger;
        private readonly ActivitesSettings _activitesSettings;
        #endregion

        #region Ctor
        public UpdateProjectCharterCommandHandler(IProjectCharterRepository projectCharterRepository, IMessageResourceReader messageResourceReader
                                                    ,IHttpContextAccessor httpContext, IProjectActivityLogger projectActivityLogger,
                                                    ActivitesSettings activitesSettings)
        {
            _projectCharterRepository = projectCharterRepository;
            _messageResourceReader = messageResourceReader;
            _httpContext = httpContext;
            _projectActivityLogger = projectActivityLogger;
            _activitesSettings = activitesSettings;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(UpdateProjectCharterCommand request, CancellationToken cancellationToken)
        {
            UserDto currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());

            ProjectCharterDto projectCharterDto = new ProjectCharterDto()
            {
                ProjectId = request.ProjectId,
                Benefits = request.Benefits,
                Costs = request.Costs,
                Deliverables = request.Deliverables,
                Goals = request.Goals,
                Misalignments = request.Misalignments,
                Scope = request.Scope,
                Summary = request.Summary
            };
            if (await _projectCharterRepository.CheckProjectCharterExist(request.ProjectId))
            {
                _projectCharterRepository.UpdateProjectCharter(projectCharterDto);
            }
            else
            {
                _projectCharterRepository.CreateProjectCharter(projectCharterDto);
            }

            if (await _projectCharterRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                await _projectActivityLogger.LogProjectActivity(_activitesSettings.UpdateProjectCharter, projectCharterDto.ProjectId, currentUser.Id, cancellationToken);
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
