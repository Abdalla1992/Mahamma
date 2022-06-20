using FluentValidation;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Workspace.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Workspace.UpdateWorkspace
{
    public class UpdateWorkspaceCommandValidator : AbstractValidator<UpdateWorkspaceCommand>
    {
        IWorkspaceRepository _workspaceRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        public UpdateWorkspaceCommandValidator(IWorkspaceRepository workspaceRepository,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _workspaceRepository = workspaceRepository;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;
            RuleFor(command => command.Id).GreaterThan(0).WithMessage(GetValidationMessage("InvalidWorkspaceId"));
            RuleFor(command => command.Name).NotEmpty().WithMessage(GetValidationMessage("NameIsEmpty"));
            RuleFor(command => command).Must(IsUniqueWorkspace).WithMessage(GetValidationMessage("NameAlreadyExists"));
        }
        private bool IsUniqueWorkspace(UpdateWorkspaceCommand updateWorkspaceCommand)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            return !(_workspaceRepository.CheckWorkspaceExistence(updateWorkspaceCommand.Name, currentUser.CompanyId, updateWorkspaceCommand.Id).Result);
        }
        private string GetValidationMessage(string Key)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            var message = _messageResourceReader.GetKeyValue(Key, currentUser.LanguageId);
            return message;
        }
    }
}
