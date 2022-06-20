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

namespace Mahamma.AppService.Task.AddTaskFile
{
    public class AddTaskFileCommandHandler : IRequestHandler<AddTaskFileCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        private readonly IProjectAttachmentRepository _projectAttachmentRepository;
        private readonly ITaskActivityLogger _taskActivityLogger;
        private readonly ActivitesSettings _taskActivitesSettings;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;

        #endregion

        #region CTRS
        public AddTaskFileCommandHandler(IProjectAttachmentRepository projectAttachmentRepository, ITaskActivityLogger taskActivityLogger,
            ActivitesSettings taskActivitesSettings, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _projectAttachmentRepository = projectAttachmentRepository;
            _taskActivityLogger = taskActivityLogger;
            _taskActivitesSettings = taskActivitesSettings;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;

        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(AddTaskFileCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            ProjectAttachment attachment = new();
            attachment.CreateAttachment(request.FileURL, request.ProjectId, request.TaskId, "");
            _projectAttachmentRepository.AddAttachment(attachment);
            if (request.TaskId != null)
            {
                await _taskActivityLogger.LogTaskActivity(_taskActivitesSettings.UploadActivity, (int)request.TaskId, currentUser.Id, cancellationToken);
            }
            if (await _projectAttachmentRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                response.Result.ResponseData = true;
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("DataAddededSuccessfully",currentUser.LanguageId);
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("GeneralError",currentUser.LanguageId);
            }
            return response;
        }
    }
}
