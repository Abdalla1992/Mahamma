using Mahamma.Base.HttpRequest.Dto;
using Mahamma.Base.HttpRequest.IService;
using Mahamma.Identity.ApiClient.Dto.Base;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Identity.ApiClient.Interface;
using Mahamma.Identity.ApiClient.Setting;
using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mahamma.Identity.ApiClient.AppService
{
    public class AccountService : IAccountService
    {
        #region CTRS
        private MahammaIdentityClientApiSetting Setting { get; }
        private IHttpHandler HttpHandler { get; }
        public AccountService(IHttpHandler httpHandler, MahammaIdentityClientApiSetting setting)
        {
            HttpHandler = httpHandler;
            Setting = setting;
        }
        #endregion
        public UserDto GetUserById(BaseRequestDto baseRequest, long id)
        {
            UserDto userDto = null;
            string url = $"{Setting.IdentityUrl}{Setting.GetUserUrl}?id={id}";
            Log.Warning($"GetUserUrl: {url}");
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", baseRequest.AuthToken);

            HttpResponseDto response = HttpHandler.GetAsync(url, headers).Result;
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrWhiteSpace(response.Content))
            {
                APIResponse<UserDto> result = JsonConvert.DeserializeObject<APIResponse<UserDto>>(response.Content);
                userDto = result?.Result?.ResponseData ?? userDto;
            }
            return userDto;
        }
        public UserDto GetUserByIdForBackgroundService(long id)
        {
            UserDto userDto = null;
            string url = $"{Setting.IdentityUrl}{Setting.GetUserUrlForBackgroundService}?id={id}";

            HttpResponseDto response = HttpHandler.GetAsync(url).Result;
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrWhiteSpace(response.Content))
            {
                APIResponse<UserDto> result = JsonConvert.DeserializeObject<APIResponse<UserDto>>(response.Content);
                userDto = result?.Result?.ResponseData ?? userDto;
            }
            return userDto;
        }
        public async Task<bool> SetUserCompany(BaseRequestDto baseRequest, int companyId)
        {
            bool done = false;
            string url = $"{Setting.IdentityUrl}{Setting.SetUserCompanyUrl}";
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", baseRequest.AuthToken);
            HttpResponseDto response = await HttpHandler.PostAsync(url, new { CompanyId = companyId }, headers);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrWhiteSpace(response.Content))
            {
                APIResponse<bool> result = JsonConvert.DeserializeObject<APIResponse<bool>>(response.Content);
                done = result?.Result.ResponseData ?? done;
            }
            return done;
        }
        public async Task<bool> UpdateUserProfileStatus(BaseRequestDto baseRequest, int userProfileStatusId)
        {
            bool done = false;
            string url = $"{Setting.IdentityUrl}{Setting.UpdateUserProfileStatusUrl}";
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", baseRequest.AuthToken);
            HttpResponseDto response = await HttpHandler.PostAsync(url, new { UserProfileStatusId = userProfileStatusId }, headers);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrWhiteSpace(response.Content))
            {
                APIResponse<bool> result = JsonConvert.DeserializeObject<APIResponse<bool>>(response.Content);
                done = result?.Result.ResponseData ?? done;
            }
            return done;
        }
    }
}
