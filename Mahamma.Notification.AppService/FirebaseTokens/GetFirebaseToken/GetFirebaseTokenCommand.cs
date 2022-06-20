using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System.Runtime.Serialization;

namespace Mahamma.Notification.AppService.FirebaseTokens.GetFirebaseToken
{
    public class GetFirebaseTokenCommand : IRequest<ValidateableResponse<ApiResponse<string>>>
    {
    }
}
