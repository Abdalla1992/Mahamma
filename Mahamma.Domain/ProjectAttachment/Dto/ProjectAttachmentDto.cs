using Mahamma.Base.Dto.Dto;
using System.Collections.Generic;

namespace Mahamma.Domain.ProjectAttachment.Dto
{
    public class ProjectAttachmentDto : BaseDto<int>
    {
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public int ProjectId { get; set; }
        public int? TaskId { get; set; }
        public int? FolderId { get; set; }
    }
}
