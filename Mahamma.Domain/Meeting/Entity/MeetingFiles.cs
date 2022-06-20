using Mahamma.Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Meeting.Entity
{
    public class MeetingFile : Entity<int>
    {

        public int MeetingId { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public void CreateMeetingFile(int meetingId, string fileURL,string name)
        {
            URL = fileURL;
            MeetingId = meetingId;
            Name = name;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }
        public void DeleteMeetingTopic()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }
    }
}
