using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.Task.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.RateTask
{
    public class RateMemberTaskCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        [DataMember]
        public int TaskId { get; set; }
        [DataMember]
        public List<TaskMemberDto> TaskMembers { get; set; }
        #endregion
    }
}
