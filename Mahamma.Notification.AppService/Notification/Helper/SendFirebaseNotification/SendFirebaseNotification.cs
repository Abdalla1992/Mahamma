using CorePush.Google;
using Mahamma.Notification.AppService.Settings;
using Mahamma.Notification.Domain.UserPushNotificationTokens.Dto;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static Mahamma.Notification.Domain.UserPushNotificationTokens.Dto.FirebaseNotificationModel;

namespace Mahamma.Notification.AppService.Notification.Helper.SendFirebaseNotification
{
    public class SendFirebaseNotification : ISendFirebaseNotification
    {
        #region Prop
        public FcmNotificationSetting _fcmNotificationSetting { get; set; }

        #endregion

        #region Ctor
        public SendFirebaseNotification(FcmNotificationSetting fcmNotificationSetting)
        {
            _fcmNotificationSetting = fcmNotificationSetting;
        }
        #endregion

        public async Task<FirebaseNotificationResponse> SendNotification(PushNotificationModel notificationModel)
        {
            FirebaseNotificationResponse response = new FirebaseNotificationResponse();
            try
            {
                if (notificationModel.IsAndroiodDevice)
                {
                    /* FCM Sender (Android Device OR Desktop App) */
                    FcmSettings settings = new FcmSettings()
                    {
                        SenderId = _fcmNotificationSetting.SenderId,
                        ServerKey = _fcmNotificationSetting.ServerKey
                    };
                    HttpClient httpClient = new HttpClient();

                    string authorizationKey = string.Format("keyy={0}", settings.ServerKey);
                    string deviceToken = notificationModel.Token;

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                    httpClient.DefaultRequestHeaders.Accept
                            .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    DataPayload dataPayload = new DataPayload();
                    dataPayload.Title = notificationModel.Title;
                    dataPayload.Body = notificationModel.Body;

                    FirebaseNotificationModel notification = new FirebaseNotificationModel();
                    notification.Data = dataPayload;
                    notification.Notification = dataPayload;

                    var fcm = new FcmSender(settings, httpClient);
                    var fcmSendResponse = await fcm.SendAsync(deviceToken, notification);

                    if (fcmSendResponse.IsSuccess())
                    {
                        response.IsSuccess = true;
                        response.Message = "Notification sent successfully";
                        return response;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = fcmSendResponse.Results[0].Error;
                        return response;
                    }
                }
                else
                {
                    /* Code here for APN Sender (iOS Device) */
                    //var apn = new ApnSender(apnSettings, httpClient);
                    //await apn.SendAsync(notification, deviceToken);
                }
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Something went wrong";
                return response;
            }
        }
    }
}
