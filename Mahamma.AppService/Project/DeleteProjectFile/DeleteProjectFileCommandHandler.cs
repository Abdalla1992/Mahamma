using Mahamma.AppService.AtivityLogger.Project;
using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.ProjectAttachment.Dto;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.ProjectAttachment.Entity;
using Mahamma.Domain.ProjectAttachment.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.DeleteProjectFile
{
    public class DeleteProjectFileCommandHandler : IRequestHandler<DeleteProjectFileCommand, ValidateableResponse<ApiResponse<List<ProjectAttachmentDto>>>>
    {
        #region Props
        private readonly IProjectAttachmentRepository _projectAttachmentRepository;
        private readonly IProjectActivityLogger _projectActivityLogger;
        private readonly ActivitesSettings _activitesSettings;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;

        #endregion

        #region Ctor

        public DeleteProjectFileCommandHandler(IProjectAttachmentRepository projectAttachmentRepository, IProjectActivityLogger projectActivityLogger,
            ActivitesSettings activitesSettings, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _projectAttachmentRepository = projectAttachmentRepository;
            _projectActivityLogger = projectActivityLogger;
            _activitesSettings = activitesSettings;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;

        }
        #endregion

        #region Methods
        public async Task<ValidateableResponse<ApiResponse<List<ProjectAttachmentDto>>>> Handle(DeleteProjectFileCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<List<ProjectAttachmentDto>>> response = new(new ApiResponse<List<ProjectAttachmentDto>>());

            ProjectAttachment projectAttachment = await _projectAttachmentRepository.GetEntityById(request.Id);
            if (projectAttachment != null)
            {
                projectAttachment.DeleteAttachment();
                _projectAttachmentRepository.UpdateProject(projectAttachment);

                await _projectActivityLogger.LogProjectActivity(_activitesSettings.RemoveProjectFileActivity, projectAttachment.ProjectId, currentUser.Id, cancellationToken);


                if (await _projectAttachmentRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    response.Result.ResponseData = (await _projectAttachmentRepository.GetProjectLatestFileList(projectAttachment.ProjectId, 3)).DataList;
                    response.Result.CommandMessage = "Data Deleted Successfully";
                }
                else
                {
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToDelete", currentUser.LanguageId);
                }
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("NoDataFound", currentUser.LanguageId);
            }
            return response;
        }

        #endregion
    }
}
