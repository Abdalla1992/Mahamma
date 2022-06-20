using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System.Runtime.Serialization;

namespace Mahamma.Notification.AppService.FirebaseTokens.AddFirebaseToken
{
    public class RemoveFirebaseTokenCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        [DataMember]
        public string FirebaseToken { get; set; }
    }
}
