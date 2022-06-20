using Mahamma.AppService.AtivityLogger.Task;
using Mahamma.AppService.Settings;
using Mahamma.AppService.Task.ListTaskFiles;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.ProjectAttachment.Entity;
using Mahamma.Domain.ProjectAttachment.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.DeleteTaskFile
{
    public class DeleteTaskFileCommandHandler : IRequestHandler<DeleteTaskFileCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        private readonly IProjectAttachmentRepository _projectAttachmentRepository;
        private readonly ITaskActivityLogger _taskActivityLogger;
        private readonly ActivitesSettings _taskActivitesSettings;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;

        #endregion

        #region CTRS
        public DeleteTaskFileCommandHandler(IProjectAttachmentRepository projectAttachmentRepository, ITaskActivityLogger taskActivityLogger,
            ActivitesSettings taskActivitesSettings, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _projectAttachmentRepository = projectAttachmentRepository;
            _taskActivityLogger = taskActivityLogger;
            _taskActivitesSettings = taskActivitesSettings;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;

        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(DeleteTaskFileCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            ProjectAttachment attachment = await _projectAttachmentRepository.GetEntityById(request.FileId);
            attachment.DeleteAttachment();
            if (attachment.TaskId != null)
            {
                await _taskActivityLogger.LogTaskActivity(_taskActivitesSettings.UploadActivity, (int)attachment.TaskId, currentUser.Id, cancellationToken);
            }

            if (await _projectAttachmentRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                response.Result.ResponseData = true;
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("DataDeletedSuccessfully", currentUser.LanguageId);
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("NoDataFound", currentUser.LanguageId);
            }
            return response;
        }
    }
}
