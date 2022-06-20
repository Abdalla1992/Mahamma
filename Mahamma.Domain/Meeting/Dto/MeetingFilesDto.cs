using Mahamma.Base.Dto.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Meeting.Dto
{
    public class MeetingFilesDto : BaseDto<int>
    {
        public int MeetingId { get; set; }
        public string URL { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public int IsDeleted { get; set; }
    }
}
