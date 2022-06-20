using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;

namespace Mahamma.Base.Domain.Event
{
    public class INotificationRequest<TResult> : INotification
    {
        public ValidateableResponse<ApiResponse<TResult>> Result { get; set; }
    }
}
