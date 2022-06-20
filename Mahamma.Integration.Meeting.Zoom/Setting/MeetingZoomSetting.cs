using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Integration.Meeting.Zoom.Setting
{
    public class MeetingZoomSetting
    {
        public string MeetingZoomBaseUrl { get; set; } = "https://api.zoom.us/v2/";
        public string AddMeetingUrl { get; set; } = "users/me/meetings";
        public string deleteMeeting { get; set; } = "meetings/";
        public string UpdateMeeting { get; set; } = "meetings/";
        public string GetMeetingById { get; set; } = "meetings/";
        public string GetMeetingListUsers { get; set; } = "users/";
        public string AllMeetingList { get; set; } = "meetings";


        public string AddSubAccountUrl { get; set; } = "/accounts";
        public string deleteSubAccount { get; set; } = "accounts/";
        public string GetSubAccountById { get; set; } = "accounts/";



        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }


    }
}
