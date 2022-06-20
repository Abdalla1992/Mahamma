﻿using FluentValidation;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Identity.ApiClient.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Workspace.DeleteWorkspace
{
    public class DeleteWorkspaceCommandValidator : AbstractValidator<DeleteWorkspaceCommand>
    {
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;

        public DeleteWorkspaceCommandValidator(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;

            RuleFor(command => command.Id).GreaterThan(0).WithMessage(GetValidationMessage("InvalidWorkspaceId"));
        }
        private string GetValidationMessage(string Key)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            var message = _messageResourceReader.GetKeyValue(Key, currentUser.LanguageId);
            return message;
        }
    }
}
