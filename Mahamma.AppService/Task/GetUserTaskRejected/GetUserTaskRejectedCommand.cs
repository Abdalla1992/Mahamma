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

namespace Mahamma.AppService.Task.GetUserTaskRejected
{
    public class GetUserTaskRejectedCommand : IRequest<ValidateableResponse<ApiResponse<List<UserTaskAcceptedRejectedDto>>>>
    {
        #region Props
        [DataMember]
        public long UserId { get; set; }

        #endregion

        #region CTRS
        public GetUserTaskRejectedCommand(long userId)
        {
            UserId = userId;
        }
        #endregion
    }
}
