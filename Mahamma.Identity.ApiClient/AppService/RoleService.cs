using Mahamma.Base.HttpRequest.Dto;
using Mahamma.Base.HttpRequest.IService;
using Mahamma.Identity.ApiClient.Dto.Base;
using Mahamma.Identity.ApiClient.Dto.Role;
using Mahamma.Identity.ApiClient.Interface;
using Mahamma.Identity.ApiClient.Setting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.ApiClient.AppService
{
    public class RoleService : IRoleService
    {
        #region CTRS
        private MahammaIdentityClientApiSetting Setting { get; }
        private IHttpHandler HttpHandler { get; }
        public RoleService(IHttpHandler httpHandler, MahammaIdentityClientApiSetting setting)
        {
            HttpHandler = httpHandler;
            Setting = setting;
        }
        #endregion
        public async Task<bool> AuthorizeUser(UserPermissionDto userPermissionDto)
        {
            bool authorized = false;
            string url = $"{Setting.IdentityUrl}{Setting.AuthorizeUserUrl}";

            HttpResponseDto response = await HttpHandler.PostAsync(url, userPermissionDto);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrWhiteSpace(response.Content))
            {
                APIResponse<bool> result = JsonConvert.DeserializeObject<APIResponse<bool>>(response.Content);
                authorized = result?.Result?.ResponseData ?? authorized;
            }
            return authorized;
        }
        public async Task<bool> SetCompanyBasicRoles(BaseRequestDto baseRequest, int companyId)
        {
            bool done = false;
            string url = $"{Setting.IdentityUrl}{Setting.SetCompanyBasicRolesUrl}";
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
    }
}
