using Mahamma.Base.HttpRequest.Dto;
using Mahamma.Base.HttpRequest.IService;
using Mahamma.Notification.ApiClient.Dto.Base;
using Mahamma.Notification.ApiClient.Dto.Notification;
using Mahamma.Notification.ApiClient.Interface;
using Mahamma.Notification.ApiClient.Setting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.ApiClient.AppService
{
    class NotificationService : INotificationService
    {
        #region CTRS
        private MahammaNotificationClientApiSetting Setting { get; }
        private IHttpHandler HttpHandler { get; }
        public NotificationService(IHttpHandler httpHandler, MahammaNotificationClientApiSetting setting)
        {
            HttpHandler = httpHandler;
            Setting = setting;
        }
        #endregion

        public async Task<bool> CreateNotification(NotificationDto notificationDto)
        {
            bool done = false;
            string url = $"{Setting.NotificationUrl}{Setting.CreateNotificationUrl}";
            HttpResponseDto response = await HttpHandler.PostAsync(url, notificationDto);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrWhiteSpace(response.Content))
            {
                ApiResponse<bool> result = JsonConvert.DeserializeObject<ApiResponse<bool>>(response.Content);
                done = result?.Result.ResponseData ?? done;
            }
            return done;
        }

        public async Task<bool> CreateNotificationList(List<NotificationDto> notificationListDto)
        {
            bool done = false;
            string url = $"{Setting.NotificationUrl}{Setting.CreateNotificationListUrl}";
            HttpResponseDto response = await HttpHandler.PostAsync(url, new { NotificationList = notificationListDto });
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrWhiteSpace(response.Content))
            {
                ApiResponse<bool> result = JsonConvert.DeserializeObject<ApiResponse<bool>>(response.Content);
                done = result?.Result?.ResponseData ?? done;
            }
            return done;
        }
    }
}
