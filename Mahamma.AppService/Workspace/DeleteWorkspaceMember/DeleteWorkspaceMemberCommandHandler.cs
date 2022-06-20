using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Workspace.Entity;
using Mahamma.Domain.Workspace.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Workspace.DeleteWorkspaceMember
{
    public class DeleteWorkspaceMemberCommandHandler : IRequestHandler<DeleteWorkspaceMemberCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        private readonly IWorkspaceMemberRepository _workspaceMemberRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        #endregion

        #region ctor
        public DeleteWorkspaceMemberCommandHandler(IWorkspaceMemberRepository workspaceMemberRepository, IMessageResourceReader messageResourceReader,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext)
        {
            _workspaceMemberRepository = workspaceMemberRepository;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(DeleteWorkspaceMemberCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            WorkspaceMember workspaceMember = await _workspaceMemberRepository.GetByWorkspaceIdAndUserId(request.WorkspaceId, request.UserId);
            if (workspaceMember != null)
            {
                _workspaceMemberRepository.DeleteWorkSpaceMember(workspaceMember);
                if (await _workspaceMemberRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("DataDeletedSuccessfully", currentUser.LanguageId);
                }
                else
                {
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("GeneralError", currentUser.LanguageId);
                }
            }
            return response;
        }
    }
}
