using Mahamma.AppService.AtivityLogger.Task;
using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Task.Entity;
using Mahamma.Domain.Task.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.DeleteComment
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        private readonly ITaskMemberRepository _taskMemberRepository;
        private readonly ITaskCommentRepository _taskCommentRepository;
        private readonly ITaskActivityLogger _taskActivityLogger;
        private readonly ActivitesSettings _taskActivitesSettings;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;

        #endregion

        #region ctor
        public DeleteCommentCommandHandler(ITaskMemberRepository taskMemberRepository, ITaskCommentRepository taskCommentRepository,
            ITaskActivityLogger taskActivityLogger, ActivitesSettings taskActivitesSettings,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _taskMemberRepository = taskMemberRepository;
            _taskCommentRepository = taskCommentRepository;
            _taskActivityLogger = taskActivityLogger;
            _taskActivitesSettings = taskActivitesSettings;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;

        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());

            TaskComment taskComment = await _taskCommentRepository.GetEntityById(request.CommentId);
            var member = await _taskMemberRepository.GetMember((int)taskComment.TaskId, currentUser.Id);
            if (member != null && member.Id == taskComment.TaskMemberId)
            {
                taskComment.DeleteComment();
                _taskActivityLogger.LogTaskActivity(_taskActivitesSettings.CommentActivity, (int)taskComment.TaskId, member.Id, cancellationToken);
                
                if (await _taskCommentRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("DataDeletedSuccessfully", currentUser.LanguageId);
                }
                else
                {
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToDelete", currentUser.LanguageId);
                }
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("CannotDeleteComment", currentUser.LanguageId);
            }
            return response;
        }
    }
}
