using FluentValidation;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Identity.ApiClient.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.AddProject
{
    class AddProjectCommandValidator : AbstractValidator<AddProjectCommand>
    {
       private readonly IProjectRepository _projectRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        public AddProjectCommandValidator(IProjectRepository projectRepository,
           Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _projectRepository = projectRepository;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;

            RuleFor(command => command.Name).NotEmpty().WithMessage(GetValidationMessage("NameIsEmpty"));
            RuleFor(command => command).Must(IsUniqueProject).WithMessage(GetValidationMessage("NameAlreadyExists"));

            RuleFor(command => command.Description).NotEmpty().WithMessage(GetValidationMessage("DescreptionIsEmpty"));
            RuleFor(command => command.DueDate).NotNull().GreaterThan(DateTime.Now.Date).WithMessage(GetValidationMessage("InvalidDueDate"));
            RuleFor(command => command.WorkSpaceId).GreaterThan(0).WithMessage(GetValidationMessage("InvalidWorkspaceId"));
        }
        
        public bool IsUniqueProject (AddProjectCommand addProjectCommand)
        {
            return !(_projectRepository.CheckProjectExistence(addProjectCommand.Name, addProjectCommand.WorkSpaceId).Result);
        }

        private string GetValidationMessage(string Key)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            var message = _messageResourceReader.GetKeyValue(Key, currentUser.LanguageId);
            return message;
        }


    }
}
