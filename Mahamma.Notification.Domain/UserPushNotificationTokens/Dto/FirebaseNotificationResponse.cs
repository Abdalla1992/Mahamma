using Newtonsoft.Json;

namespace Mahamma.Notification.Domain.UserPushNotificationTokens.Dto
{
    public class FirebaseNotificationResponse
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
