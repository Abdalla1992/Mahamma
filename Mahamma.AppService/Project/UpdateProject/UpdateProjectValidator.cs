using FluentValidation;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Domain.Workspace.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.UpdateProject
{
    public class UpdateProjectValidator : AbstractValidator<UpdateProjectCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        public UpdateProjectValidator(IProjectRepository projectRepository,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _projectRepository = projectRepository;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;

            RuleFor(command => command.Id).GreaterThan(0).WithMessage(GetValidationMessage("InvalidProjectId"));
            RuleFor(command => command.Name).NotEmpty().WithMessage(GetValidationMessage("NameIsEmpty"));
            RuleFor(command => command).Must(IsUniqueProject).WithMessage(GetValidationMessage("NameAlreadyExists"));
            RuleFor(command => command.Description).NotEmpty().WithMessage(GetValidationMessage("DescreptionIsEmpty"));
            RuleFor(command => command.DueDate).NotNull().GreaterThanOrEqualTo(DateTime.Now.Date).WithMessage(GetValidationMessage("InvalidDueDate"));
            RuleFor(command => command.WorkSpaceId).GreaterThan(0).WithMessage(GetValidationMessage("InvalidWorkspaceId"));

        }
        private bool IsUniqueProject(UpdateProjectCommand updateProjectCommand)
        {
            return !(_projectRepository.CheckProjectExistence(updateProjectCommand.Name, updateProjectCommand.WorkSpaceId, updateProjectCommand.Id).Result);
        }
        private string GetValidationMessage(string Key)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            var message = _messageResourceReader.GetKeyValue(Key, currentUser.LanguageId);
            return message;
        }

        //private bool IsWorkSpaceValid(UpdateProjectCommand updateProjectCommand)
        //{
        //    return (_workspaceRepository.CheckWorkSpaceValid(updateProjectCommand.WorkSpaceId).Result);
        //}
    }
}
