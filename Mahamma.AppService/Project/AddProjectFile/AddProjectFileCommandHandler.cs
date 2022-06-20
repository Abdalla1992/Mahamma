using Mahamma.AppService.AtivityLogger.Project;
using Mahamma.AppService.AtivityLogger.Task;
using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.ProjectAttachment.Dto;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.ProjectAttachment.Entity;
using Mahamma.Domain.ProjectAttachment.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.AddProjectFile
{
    public class AddProjectFileCommandHandler : IRequestHandler<AddProjectFileCommand, ValidateableResponse<ApiResponse<List<ProjectAttachmentDto>>>>
    {
        #region Props
        private readonly IProjectAttachmentRepository _projectAttachmentRepository;
        private readonly IProjectActivityLogger _projectActivityLogger;
        private readonly ActivitesSettings _activitesSettings;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly IFolderFileRepository _folderFileRepository;

        private readonly ITaskActivityLogger _taskActivityLogger;
        private readonly ActivitesSettings _taskActivitesSettings;
        #endregion

        #region Ctor

        public AddProjectFileCommandHandler(IProjectAttachmentRepository projectAttachmentRepository, IProjectActivityLogger projectActivityLogger,
            ActivitesSettings activitesSettings, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext,
            ITaskActivityLogger taskActivityLogger, ActivitesSettings taskActivitesSettings, IMessageResourceReader messageResourceReader, IFolderFileRepository folderFileRepository)
        {
            _projectAttachmentRepository = projectAttachmentRepository;
            _projectActivityLogger = projectActivityLogger;
            _activitesSettings = activitesSettings;
            _httpContext = httpContext;
            _taskActivityLogger = taskActivityLogger;
            _taskActivitesSettings = taskActivitesSettings;
            _messageResourceReader = messageResourceReader;
            _folderFileRepository = folderFileRepository;

        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<List<ProjectAttachmentDto>>>> Handle(AddProjectFileCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<List<ProjectAttachmentDto>>> response = new(new ApiResponse<List<ProjectAttachmentDto>>());

            ProjectAttachment projectAttachment = null;
            foreach (var uploadedFile in request.UploadedFiles)
            {
                projectAttachment = new();
                projectAttachment.CreateAttachment(uploadedFile.FileUrl, request.ProjectId, request.TaskId, uploadedFile.ActualFileName);
            }

            _projectAttachmentRepository.AddAttachment(projectAttachment);

            await _projectActivityLogger.LogProjectActivity(_activitesSettings.UploadProjectFile, request.ProjectId, currentUser.Id, cancellationToken);
            if (request.TaskId.HasValue)
                await _taskActivityLogger.LogTaskActivity(_taskActivitesSettings.UploadActivity, (int)request.TaskId, currentUser.Id, cancellationToken);

            if (await _projectAttachmentRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                if (!request.TaskId.HasValue)
                    response.Result.ResponseData = (await _projectAttachmentRepository.GetProjectLatestFileList(request.ProjectId, 3)).DataList;
                else
                    response.Result.ResponseData = (await _projectAttachmentRepository.GetTaskLatestFileList(request.TaskId, 3)).DataList;
                response.Result.CommandMessage = "Files Added Successfully";

                // to add FileId And ProjectAttachmentId In FolderFileEntity 
                List<FolderFile> folderFiles = new();
                if (request.FolderId != null)
                {
                        FolderFile folderFile = new();
                        folderFile.CreateFolderFile((int)request.FolderId, projectAttachment.Id);
                        folderFiles.Add(folderFile);
                }
                _folderFileRepository.AddFolderFiles(folderFiles);
                await _folderFileRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("GeneralError", currentUser.LanguageId);
            }
            return response;
        }
    }
}
