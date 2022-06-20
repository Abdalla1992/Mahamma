using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.ProjectAttachment.Dto;
using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mahamma.AppService.Task.ListTaskFiles
{
    public class ListTaskFilesQuery : IRequest<ValidateableResponse<ApiResponse<List<ProjectAttachmentDto>>>>
    {
        #region Props
        [DataMember]
        public int TaskId { get; set; }
        #endregion

        #region CTRS
        public ListTaskFilesQuery(int id)
        {
            TaskId = id;
        }
        #endregion
    }
}
