using Mahamma.Base.HttpRequest.Dto;
using Mahamma.Base.HttpRequest.IService;
using Mahamma.Integration.Meeting.Zoom.Dto.Request;
using Mahamma.Integration.Meeting.Zoom.Dto.Response;
using Mahamma.Integration.Meeting.Zoom.IService;
using Mahamma.Integration.Meeting.Zoom.Setting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Integration.Meeting.Zoom.Service
{
    public class SubAccountService : ISubAccountService
    {
        #region Prop
        private MeetingZoomSetting _meetingZoomSetting { get; }
        private IHttpHandler _httpHandler { get; }
        #endregion

        #region Ctor
        public SubAccountService(MeetingZoomSetting meetingZoomSetting, IHttpHandler httpHandler)
        {
            _meetingZoomSetting = meetingZoomSetting;
            _httpHandler = httpHandler;
        }
        #endregion

        public async Task<SubAccountResponseDto> AddMeeting(SubAccountRequestDto subAccountDto)
        {
            SubAccountResponseDto result = new(); ;
            string url = $"{_meetingZoomSetting.MeetingZoomBaseUrl}{_meetingZoomSetting.AddSubAccountUrl}";
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", string.Format("Bearer {0}", ZoomToken()));
            HttpResponseDto response = await _httpHandler.PostAsync(url, subAccountDto, headers);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.Created && !string.IsNullOrWhiteSpace(response.Content))
            {
                result = JsonConvert.DeserializeObject<SubAccountResponseDto>(response.Content);
            }
            return result;
        }


        public async Task<bool> DeleteSubAccount(long subAccountId)
        {
            string url = $"{_meetingZoomSetting.MeetingZoomBaseUrl}{_meetingZoomSetting.deleteSubAccount}{subAccountId}";
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", string.Format("Bearer {0}", ZoomToken()));
            HttpResponseDto response = await _httpHandler.DeleteAsync(url, headers);
            return response.StatusCode == System.Net.HttpStatusCode.NoContent || response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
        public SubAccountResponseDto GetById(long subAccountId)
        {
            SubAccountResponseDto subAccountResponseDto = new();
            string url = $"{_meetingZoomSetting.MeetingZoomBaseUrl}{_meetingZoomSetting.GetSubAccountById}{subAccountId}";
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", string.Format("Bearer {0}", ZoomToken()));
            HttpResponseDto response = _httpHandler.GetAsync(url, headers).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrWhiteSpace(response.Content))
            {
                subAccountResponseDto = JsonConvert.DeserializeObject<SubAccountResponseDto>(response.Content);
            }
            return subAccountResponseDto;
        }





        private string ZoomToken()
        {
            // Token will be good for 20 minutes
            DateTime Expiry = DateTime.UtcNow.AddMinutes(1440);

            int ts = (int)(Expiry - new DateTime(1970, 1, 1)).TotalSeconds;
            string apiSecret = _meetingZoomSetting.ApiSecret;
            string apiKey = _meetingZoomSetting.ApiKey;
            // Create Security key using private key above:
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiSecret));

            // I did changes in below line because DLL needed HmacSha256Signature instead of HmacSha256
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Finally create a Token
            var header = new JwtHeader(credentials);

            //Zoom Required Payload
            var payload = new JwtPayload { { "iss", apiKey }, { "exp", ts }, };

            var secToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();

            // Token to String so you can use it in your client
            var tokenString = handler.WriteToken(secToken);

            return tokenString;
        }

     
    }
}
