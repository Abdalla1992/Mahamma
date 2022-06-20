using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.ProjectAttachment.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.AddProjectFile
{
    public class AddProjectFileCommand : IRequest<ValidateableResponse<ApiResponse<List<ProjectAttachmentDto>>>>
    {
        #region Props
        [DataMember]
        public int ProjectId { get; set; }
        [DataMember]
        public int? TaskId { get; set; }
        [DataMember]
        public List<UploadedFileDto> UploadedFiles { get; set; }
        [DataMember]
        public int? FolderId { get; set; }
        #endregion
    }
}
