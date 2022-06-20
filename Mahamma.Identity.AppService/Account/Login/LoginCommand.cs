using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.Domain.User.Dto;
using MediatR;
using System.Runtime.Serialization;

namespace Mahamma.Identity.AppService.Account.Login.LoginCommand
{
    public class LoginCommand : IRequest<ValidateableResponse<ApiResponse<UserDto>>>
    {
        #region Props
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Password { get; set; }
        #endregion
    }
}
