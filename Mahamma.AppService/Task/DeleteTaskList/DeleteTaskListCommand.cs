using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using System.Runtime.Serialization;
using MediatR;
using System.Collections.Generic;

namespace Mahamma.AppService.Task.DeleteTaskList
{
    public class DeleteTaskListCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        [DataMember]
        public List<int> TaskIdList { get; set; }
        #endregion
    }
}
