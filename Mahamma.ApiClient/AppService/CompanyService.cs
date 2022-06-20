using Mahamma.ApiClient.Dto.Base;
using Mahamma.ApiClient.Dto.Company;
using Mahamma.ApiClient.Interface;
using Mahamma.ApiClient.Setting;
using Mahamma.Base.HttpRequest.Dto;
using Mahamma.Base.HttpRequest.IService;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mahamma.ApiClient.AppService
{
    public class CompanyService : ICompanyService
    {
        #region CTRS
        private MahammaClientApiSetting Setting { get; }
        private IHttpHandler HttpHandler { get; }
        public CompanyService(IHttpHandler httpHandler, MahammaClientApiSetting setting)
        {
            HttpHandler = httpHandler;
            Setting = setting;
        }
        #endregion
        public async Task<CompanyDto> GetCompanyById(int id, string authToken)
        {
            CompanyDto companyDto = null;
            string url = $"{Setting.MahammaApiUrl}{Setting.GetCompanyUrl}?id={id}";
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", authToken);

            HttpResponseDto response = await HttpHandler.GetAsync(url, headers);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrWhiteSpace(response.Content))
            {
                APIResponse<CompanyDto> result = JsonConvert.DeserializeObject<APIResponse<CompanyDto>>(response.Content);
                companyDto = result?.Result?.ResponseData ?? companyDto;
            }
            return companyDto;
        }

        public async Task<bool> UpdateInvitationStatus(BaseRequestDto baseRequest, string invitationId, int invitationStatusId)
        {
            bool done = false;
            string url = $"{Setting.MahammaApiUrl}{Setting.UpdateInvitationStatusUrl}";
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", baseRequest.AuthToken);

            HttpResponseDto response = await HttpHandler.PostAsync(url, new { InvitationId = invitationId, InvitationStatusId = invitationStatusId }, headers);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrWhiteSpace(response.Content))
            {
                APIResponse<bool> result = JsonConvert.DeserializeObject<APIResponse<bool>>(response.Content);
                done = result.IsValidResponse ? result.Result.ResponseData : false;
            }
            return done;
        }
        public async Task<CompanyInvitationDto> GetCompanyInvitation(string invitationId)
        {
            CompanyInvitationDto companyInvitationDto = null;
            string url = $"{Setting.MahammaApiUrl}{Setting.GetCompanyInvitationUrl}?invitationId={invitationId}";

            HttpResponseDto response = await HttpHandler.GetAsync(url);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrWhiteSpace(response.Content))
            {
                APIResponse<CompanyInvitationDto> result = JsonConvert.DeserializeObject<APIResponse<CompanyInvitationDto>>(response.Content);
                companyInvitationDto = result?.Result?.ResponseData ?? companyInvitationDto;
            }
            return companyInvitationDto;
        }
    }
}
