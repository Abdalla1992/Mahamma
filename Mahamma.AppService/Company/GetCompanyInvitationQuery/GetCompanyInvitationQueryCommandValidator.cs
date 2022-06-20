using FluentValidation;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Identity.ApiClient.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Company.GetCompanyInvitationQuery
{
    public class GetCompanyInvitationQueryCommandValidator : AbstractValidator<GetCompanyInvitationQueryCommand>
    {
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        public GetCompanyInvitationQueryCommandValidator(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;
            RuleFor(command => command.InvitationId).NotEmpty().WithMessage("Invitation id is required");
        }
    }
}
