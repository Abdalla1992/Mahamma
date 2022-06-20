using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Notification.Domain.UserPushNotificationTokens.Entity;
using Mahamma.Notification.Domain.UserPushNotificationTokens.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.FirebaseTokens.GetFirebaseToken
{
    public class GetFirebaseTokenCommandHandler : IRequestHandler<GetFirebaseTokenCommand, ValidateableResponse<ApiResponse<string>>>
    {
        #region Prop
        private readonly IFirebaseNotificationTokensRepository _firebaseTokensRepository;
        private readonly IHttpContextAccessor _httpContext;
        #endregion

        #region Ctor
        public GetFirebaseTokenCommandHandler(IFirebaseNotificationTokensRepository firebaseNotificationTokensRepository, IHttpContextAccessor httpContext)
        {
            _firebaseTokensRepository = firebaseNotificationTokensRepository;
            _httpContext = httpContext;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<string>>> Handle(GetFirebaseTokenCommand request, CancellationToken cancellationToken)
        {
            string currentUserId = (string)_httpContext.HttpContext.Items["UserId"];

            ValidateableResponse<ApiResponse<string>> response = new(new ApiResponse<string>());
            FirebaseNotificationTokens firebaseTokens = await _firebaseTokensRepository.GetFirebaseNotificationToken(long.Parse(currentUserId));
            if (firebaseTokens != null)
            {
                response.Result.ResponseData = firebaseTokens.FirebaseToken;
                response.Result.CommandMessage = "Data Retrieved Successfully";
            }
            else
            {
                response.Result.CommandMessage = "Failed to Retrieve the token. Try again shortly.";
            }
            return response;
        }
    }
}
