using Mahamma.AppService.AtivityLogger.Project;
using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Project.Entity;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.DeleteProjectComment
{
    public class DeleteProjectCommentCommandHandler : IRequestHandler<DeleteProjectCommentCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IProjectCommentRepository _projectCommentRepository;
        private readonly IProjectActivityLogger _projectActivityLogger;
        private readonly ActivitesSettings _activitesSettings;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        #endregion


        #region ctor
        public DeleteProjectCommentCommandHandler(IProjectMemberRepository projectMemberRepository, IProjectCommentRepository projectCommentRepository,
            IProjectActivityLogger projectActivityLogger, ActivitesSettings activitesSettings,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _projectMemberRepository = projectMemberRepository;
            _projectCommentRepository = projectCommentRepository;
            _projectActivityLogger = projectActivityLogger;
            _activitesSettings = activitesSettings;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;

        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(DeleteProjectCommentCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());

            ProjectComment projectComment = await _projectCommentRepository.GetEntityById(request.CommentId);
            var member = await _projectMemberRepository.GetMemberByIdForMakeComment((int)projectComment.ProjectId, currentUser.Id);
            if (member != null && member.Id == projectComment.ProjectMemberId)
            {
                projectComment.DeleteComment();
                _projectActivityLogger.LogProjectActivity(_activitesSettings.DeleteProjectComment, (int)projectComment.ProjectId, member.Id, cancellationToken);

                if (await _projectCommentRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = "Data Deleted Successfully";
                }
                else
                {
                    response.Result.CommandMessage = "Failed To Delete";
                }
            }
            else
            {
                response.Result.CommandMessage = "Cannot Delete Comment";
            }
            return response;
        }
    }
}
