using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Notification.Domain.UserPushNotificationTokens.Entity;
using Mahamma.Notification.Domain.UserPushNotificationTokens.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.FirebaseTokens.AddFirebaseToken
{
    public class AddFirebaseTokenCommandHandler : IRequestHandler<AddFirebaseTokenCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly IFirebaseNotificationTokensRepository _firebaseTokensRepository;
        private readonly IHttpContextAccessor _httpContext;
        #endregion

        #region Ctor
        public AddFirebaseTokenCommandHandler(IFirebaseNotificationTokensRepository firebaseNotificationTokensRepository, IHttpContextAccessor httpContext)
        {
            _firebaseTokensRepository = firebaseNotificationTokensRepository;
            _httpContext = httpContext;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(AddFirebaseTokenCommand request, CancellationToken cancellationToken)
        {
            string currentUserId = (string)_httpContext.HttpContext.Items["UserId"];

            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            FirebaseNotificationTokens firebaseTokens = new();
            firebaseTokens.CreateuserFirebaseToken(request.FirebaseToken, long.Parse(currentUserId));
            _firebaseTokensRepository.AddFirebaseToken(firebaseTokens);
            if (await _firebaseTokensRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                response.Result.ResponseData = true;
                response.Result.CommandMessage = "Data Added Successfully";
            }
            else
            {
                response.Result.CommandMessage = "Failed to add the new Notification token. Try again shortly.";
            }
            return response;
        }
    }
}
