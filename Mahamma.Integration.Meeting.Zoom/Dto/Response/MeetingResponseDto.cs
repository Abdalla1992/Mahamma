using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Integration.Meeting.Zoom.Dto.Response
{
    public class MeetingResponseDto
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }
        [JsonProperty("id")]
        public long MeetingId { get; set; }
        [JsonProperty("join_url")]
        public string JoinUrl { get; set; }
        [JsonProperty("start_time")]
        public string StartTime { get; set; }
        [JsonProperty("duration")]
        public int Duration { get; set; }
        [JsonProperty("timezone")]
        public string TimeZone { get; set; }
        [JsonProperty("agenda")]
        public string Agenda { get; set; }
        [JsonProperty("pre_schedule")]
        public string PreSchedule { get; set; }
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
        [JsonProperty("topic")]
        public string topic { get; set; }
    }

    public class MeetingResponseListDto
    {
        [JsonProperty("page_count")]
        public int PageCount { get; set; }
        [JsonProperty("page_size")]
        public int PageSize { get; set; }
        [JsonProperty("page_number")]
        public int PageNumber { get; set; }
        [JsonProperty("total_records")]
        public int TotalRecords { get; set; }
        [JsonProperty("next_page_token")]
        public string NextPageToken { get; set; }
        [JsonProperty("meetings")]
        public List<MeetingResponseDto> meetingResponseDto { get; set; }
    }
}
