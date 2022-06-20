using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.TaskActivity.Dto;
using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mahamma.AppService.Task.ListTaskLogs
{
    public class ListTaskLogsQuery : IRequest<ValidateableResponse<ApiResponse<List<TaskActivityDto>>>>
    {
        #region Props
        [DataMember]
        public int TaskId { get; set; }
        #endregion

        #region CTRS
        public ListTaskLogsQuery(int id)
        {
            TaskId = id;
        }
        #endregion
    }
}
