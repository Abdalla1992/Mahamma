using Mahamma.Base.HttpRequest.Dto;
using Mahamma.Base.HttpRequest.IService;
using Mahamma.Integration.Meeting.Zoom.Dto;
using Mahamma.Integration.Meeting.Zoom.IService;
using Mahamma.Integration.Meeting.Zoom.Setting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.IdentityModel.Tokens;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using Mahamma.Integration.Meeting.Zoom.Dto.Response;

namespace Mahamma.Integration.Meeting.Zoom.Service
{
    public class MeetingService : IMeetingService
    {
        #region Prop
        private  MeetingZoomSetting _meetingZoomSetting { get; }
        private IHttpHandler _httpHandler { get; }
        #endregion

        #region Ctor
        public MeetingService(MeetingZoomSetting meetingZoomSetting,IHttpHandler httpHandler)
        {
            _meetingZoomSetting = meetingZoomSetting;
            _httpHandler = httpHandler;
        }

        #endregion

        #region Methods

        public async Task<MeetingResponseDto> AddMeeting(MeetingRequestDto meetingDto)
        {

            MeetingResponseDto result = new();;
            string url =$"{_meetingZoomSetting.MeetingZoomBaseUrl}{_meetingZoomSetting.AddMeetingUrl}";
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", string.Format("Bearer {0}", ZoomToken()));
            HttpResponseDto response =await _httpHandler.PostAsync(url,meetingDto, headers);
            if (response != null  && response.StatusCode == System.Net.HttpStatusCode.Created && !string.IsNullOrWhiteSpace(response.Content))
            {
                result = JsonConvert.DeserializeObject<MeetingResponseDto>(response.Content);
            }
            return result;
        }

        public MeetingResponseDto GetById(long meetingId)
        {
            MeetingResponseDto meetingResponseDto = new();
            string url = $"{_meetingZoomSetting.MeetingZoomBaseUrl}{_meetingZoomSetting.GetMeetingById}{meetingId}";
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", string.Format("Bearer {0}", ZoomToken()));
            HttpResponseDto response =  _httpHandler.GetAsync(url,headers).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrWhiteSpace(response.Content))
            {
                meetingResponseDto = JsonConvert.DeserializeObject<MeetingResponseDto>(response.Content);
            }
            return meetingResponseDto;
        }

        public async Task<bool> UpdateMeeting(long meetingId,MeetingResponseDto meetingDto)
        {
            bool result = false;
            string url = $"{_meetingZoomSetting.MeetingZoomBaseUrl}{_meetingZoomSetting.UpdateMeeting}{meetingId}";
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", string.Format("Bearer {0}", ZoomToken()));
            HttpResponseDto response = await _httpHandler.PatchAsync(url, meetingDto, headers);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                result = true;
            }
            return result;
        }

        public async Task<bool> DeleteMeeting(long meetingId)
        {
            string url = $"{_meetingZoomSetting.MeetingZoomBaseUrl}{_meetingZoomSetting.deleteMeeting}{meetingId}";
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", string.Format("Bearer {0}", ZoomToken()));
            HttpResponseDto response = await _httpHandler.DeleteAsync(url,headers);
            return response.StatusCode == System.Net.HttpStatusCode.NoContent || response.StatusCode == System.Net.HttpStatusCode.NoContent;

        }

        public async Task<List<MeetingResponseDto>> GetAll(string userId)
        {
            List<MeetingResponseDto> meetingResponseDtoResult = new List<MeetingResponseDto>();
            string url = $"{_meetingZoomSetting.MeetingZoomBaseUrl}{_meetingZoomSetting.GetMeetingListUsers}{userId}/{_meetingZoomSetting.AllMeetingList}";
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", string.Format("Bearer {0}", ZoomToken()));
            HttpResponseDto response = await _httpHandler.GetAsync(url, headers);
            if (response.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrWhiteSpace(response.Content))
            {
                MeetingResponseListDto listData = JsonConvert.DeserializeObject<MeetingResponseListDto>(response.Content);
                meetingResponseDtoResult =listData.meetingResponseDto;
            }

            return meetingResponseDtoResult;
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
        #endregion
    }
}
