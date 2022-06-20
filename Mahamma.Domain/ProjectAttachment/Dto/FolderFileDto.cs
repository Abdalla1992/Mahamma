using Mahamma.Base.Dto.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.ProjectAttachment.Dto
{
    public class FolderFileDto : BaseDto<int>
    {
        public int FolderId { get; set; }
        public int FileId { get; set; }
    }
}
