using FluentValidation;
using Mahamma.Base.Resources.IResourceReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.AddProjectComment
{
    public class AddProjectCommentCommandValidator : AbstractValidator<AddProjectCommentCommand>
    {
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        public AddProjectCommentCommandValidator(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;
            RuleFor(command => command.ProjectId).GreaterThan(0).WithMessage("invalid ProjectId");
            RuleFor(command => command.Comment).NotEmpty().WithMessage("Invalid Project Name");
        }
    }
}
