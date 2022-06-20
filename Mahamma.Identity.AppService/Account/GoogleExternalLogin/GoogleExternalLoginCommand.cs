using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.Domain.User.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Account.GoogleExternalLogin
{
    public class GoogleExternalLoginCommand : IRequest<ValidateableResponse<ApiResponse<UserDto>>>
    {
        public string Provider { get; set; }
        public string IdToken { get; set; }
        public string FullName { get; set; }
        public string InvitationId { get; set; }
    }
}
