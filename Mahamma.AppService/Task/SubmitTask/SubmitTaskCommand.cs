using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mahamma.AppService.Task.SubmitTask
{
    public class SubmitTaskCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        [DataMember]
        public int TaskId { get; set; }
        [DataMember]
        public int DurationInHours { get; set; }
        #endregion
    }
}
