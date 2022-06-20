using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.Workspace.Entity;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Workspace.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Workspace.DeleteWorkspace
{
    public class DeleteWorkspaceCommandHandler : IRequestHandler<DeleteWorkspaceCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        private readonly IWorkspaceRepository _workspaceRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;


        private readonly IWorkspaceMemberRepository _workspaceMemberRepository;
        #endregion

        #region CTRS
        public DeleteWorkspaceCommandHandler(IWorkspaceRepository workspaceRepository, IWorkspaceMemberRepository workspaceMemberRepository, IMessageResourceReader messageResourceReader,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext)
        {
            _workspaceRepository = workspaceRepository;
            _messageResourceReader = messageResourceReader;
            _httpContext = httpContext;

            _workspaceMemberRepository = workspaceMemberRepository;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(DeleteWorkspaceCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());

            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];

            Domain.Workspace.Entity.Workspace workspace = await _workspaceRepository.GetWorkspaceById(request.Id);

            if (workspace != null)
            {
                workspace.DeleteWorkspace();

                _workspaceRepository.UpdateWorkspace(workspace);
                List<WorkspaceMember> workspaceMembers = await _workspaceMemberRepository.GetWorkSpaceMemberById(request.Id);
                if(workspaceMembers?.Count>0)
                {
                    foreach (var member in workspaceMembers)
                    {
                        member.DeleteWorkspaceMember();
                    }
                    _workspaceMemberRepository.UpdateWorkSpaceMemberList(workspaceMembers);
                }
                if (await _workspaceRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    response.Result.ResponseData = true;
                    //to Add MessageResourceReader with its layers
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("DataDeletedSuccessfully",currentUser.LanguageId);
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

    }
}
