using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Mahamma.Integration.Meeting.Zoom.Dto
{
    public class MeetingRequestDto
    {
        [JsonProperty("topic")]
        public string Topic { get; set; }
        [JsonProperty("type")]
        public MeetingTypes Type { get; set; }
        [JsonProperty("start_time")]
        public string StartTime { get; set; }
        [JsonProperty("duration")]
        public int Duration { get; set; }
        //public string Timezone { get; set; }
        //[JsonProperty("password")]
        //public string Password { get; set; }
        [JsonProperty("agenda")]
        public string Agenda { get; set; }
        //[JsonProperty("recurrence")]
        //public MeetingRecurrenceDto Recurrence { get; set; }
        //[JsonProperty("settings")]
        //public MeetingSettingsDto Settings { get; set; }

    }

    public class MeetingRecurrenceDto
    {

        [JsonProperty("weekly_days")]
        public string WeeklyDaysList{ get; set; }
        [JsonProperty("type")]
        public MeetingRecurrenceTypes Type { get; set; }
        [JsonProperty("repeat_interval")]
        public int RepeatInterval { get; set; }
        [JsonProperty("monthly_day")]
        public int MonthlyDay { get; set; }
        [JsonProperty("monthly_week")]
        public MeetingRecurrenceWeeks MonthlyWeek { get; set; }

        [JsonProperty("monthly_week_day")]
        public MeetingRecurrenceWeekDays MonthlyWeekDay { get; set; }

        [JsonProperty("end_times")]
        public int EndTimes { get; set; }

        [JsonProperty("end_date_time")]
        public string EndDateTime { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public List<MeetingRecurrenceWeekDays> WeeklyDays
        {
            get
            {
                if (string.IsNullOrWhiteSpace(WeeklyDaysList))
                {
                    return null;
                }
                return WeeklyDaysList.Split(',').Select(e => (MeetingRecurrenceWeekDays)Enum.Parse(typeof(MeetingRecurrenceWeekDays), e))?.ToList();
            }
            set
            {
                WeeklyDaysList = string.Join(",", value.ToString());
            }
        }
    }
    public class MeetingSettingsDto
    {

        [JsonProperty("host_video")]
        public bool EnableHostVideo { get; set; }

        //[JsonProperty("participant_video")]
        //public bool EnableParticipantVideo { get; set; }

        //[JsonProperty("cn_meeting")]
        //public bool EnableChinaHost { get; set; }
        [JsonProperty("waiting_room")]
        public bool WaitingRoom { get; set; }
        //[JsonProperty("in_meeting")]
        //public bool EnableIndiaHost { get; set; }

  
        //[JsonProperty("join_before_host")]
        //public bool EnableJoinBeforeHost { get; set; }

        //[JsonProperty("mute_upon_entry")]
        //public bool EnableMuteOnEntry { get; set; }

  
        //[JsonProperty("watermark")]
        //public bool EnableWatermark { get; set; }

  
        //[JsonProperty("use_pmi")]
        //public bool UsePersonalMeetingId { get; set; }

   
        //public MeetingApprovalTypes ApprovalType { get; set; }

   
        //public MeetingRegistrationTypes RegistrationType { get; set; }

        //public string Audio { get; set; }
        [JsonProperty("auto_recording")]
        public string AutoRecording { get; set; }

 
        //[JsonProperty("enforce_login")]
        //public bool EnableEnforceLogin { get; set; }

    
        //[JsonProperty("enforce_login_domains")]
        //public string EnableEnforceLoginDomains { get; set; }

   
        //public string AlternativeHosts { get; set; }
    }

    public enum MeetingRecurrenceWeekDays
    {
        Sunday = 1,
        Monday = 2,
        Tuesday = 3,
        Wednesday = 4,
        Thursday = 5,
        Friday = 6,
        Saturday = 7
    }
    public enum MeetingRecurrenceWeeks
    {
        Last = -1,
        First = 1,
        Second = 2,
        Third = 3,
        Fourth = 4
    }
    public enum MeetingRecurrenceTypes
    {
        Daily = 1,
        Weekly = 2,
        Monthly = 3
    }
    public enum MeetingTypes
    {
        Instant = 1,
        Scheduled = 2,
        RecurringNoTime = 3,
        RecurringWithTime = 4
    }
    public enum MeetingApprovalTypes
    {
        Automatic = 0,
        Manual = 1,
        NoRegistration = 2
    }
    public enum MeetingRegistrationTypes
    {
        RegisterAllOccurrences = 1,
        RegisterEachOccurrence = 2,
        RegisterChooseOccurrence = 3
    }
}
