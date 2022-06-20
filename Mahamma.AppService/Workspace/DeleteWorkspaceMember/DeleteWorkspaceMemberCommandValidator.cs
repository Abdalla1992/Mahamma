using FluentValidation;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Workspace.Entity;
using Mahamma.Domain.Workspace.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Notification.ApiClient.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Workspace.DeleteWorkspaceMember
{
    public class DeleteWorkspaceMemberCommandValidator : AbstractValidator<DeleteWorkspaceMemberCommand>
    {
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly IWorkspaceRepository _workspaceRepository;
        public DeleteWorkspaceMemberCommandValidator(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext,
            IMessageResourceReader messageResourceReader, IWorkspaceRepository workspaceRepository)
        {
            _httpContext = httpContext;
            _workspaceRepository = workspaceRepository;
            _messageResourceReader = messageResourceReader;
            RuleFor(command => command.UserId).GreaterThan(0).WithMessage(GetValidationMessage("InvalidUserId"));
            RuleFor(command => command.WorkspaceId).GreaterThan(0).WithMessage(GetValidationMessage("InvalidWorkspaceId"));
            RuleFor(command => command).Must(IsDeletedMemeberSameCreator).WithMessage(GetValidationMessage("MemberSameAsCreatorUser"));
        }

        private string GetValidationMessage(string Key)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            var message = _messageResourceReader.GetKeyValue(Key, currentUser != null ? currentUser.LanguageId : Language.English.Id);
            return message;
        }

        private bool IsDeletedMemeberSameCreator(DeleteWorkspaceMemberCommand deleteWorkspaceMemberCommand)
        {
            Domain.Workspace.Entity.Workspace workspace = _workspaceRepository.GetWorkspaceById(deleteWorkspaceMemberCommand.WorkspaceId).Result;
            if (workspace != null)
            {
                if (workspace.CreatorUserId == deleteWorkspaceMemberCommand.UserId) return false;
                else return true;
            }
            else return false;
        }
    }
}
