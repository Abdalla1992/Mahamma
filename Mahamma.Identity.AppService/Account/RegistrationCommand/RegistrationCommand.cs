using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.Domain.User.Dto;
using MediatR;
using System.Runtime.Serialization;

namespace Mahamma.AppService.Task.AddComment
{
    public class RegistrationCommand : IRequest<ValidateableResponse<ApiResponse<UserDto>>>
    {
        #region Props
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Password { get; set; } 
        [DataMember]
        public string ConfirmPassword { get; set; }
        [DataMember]
        public string InvitationId { get; set; }
        #endregion
    }
}
